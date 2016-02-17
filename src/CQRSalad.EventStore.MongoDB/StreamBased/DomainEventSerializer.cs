﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using CQRSalad.Dispatching.Async;
using CQRSalad.EventStore.Core;
using CQRSalad.EventStore.MongoDB.Serializers;
using MongoDB.Bson;

namespace CQRSalad.EventStore.MongoDB
{
    internal class DomainEventSerializer
    {
        private readonly MongoDataSerializer _dataSerializer;

        private const string BodyElement = "Body";
        private const string MetaElement = "Meta";
        private const string CLRTypeElement = "CLR_Type";
        private const string TimestampElement = "Timestamp";
        private const string SenderElement = "SenderId";

        public DomainEventSerializer()
        {
            _dataSerializer = new MongoDataSerializer();
        }

        public BsonDocument Serialize(DomainEvent @event)
        {
            return new BsonDocument()
            {
                {
                    BodyElement, _dataSerializer.Serialize(@event.Body)
                },
                {
                    MetaElement, new BsonDocument()
                    {
                        { CLRTypeElement, @event.Body.GetType().AssemblyQualifiedName },
                        { TimestampElement, @event.Meta.Timestamp },
                        { SenderElement, @event.Meta.SenderId }
                    }
                }
            };
        }

        public BsonArray SerializeMany(IEnumerable<DomainEvent> events)
        {
            var array = new BsonArray();
            foreach (DomainEvent evnt in events)
            {
                array.Add(Serialize(evnt));
            }
            return array;
        }

        public DomainEvent Deserialize(BsonDocument bsonDocument, string root)
        {
            string typeName = bsonDocument[$"{MetaElement}"][$"{CLRTypeElement}"].AsString;
            Type eventType = Type.GetType(typeName);
            if (eventType == null)
            {
                throw new SerializationException($"Cannot load type: {typeName}. Make sure that assembly containing this type is referenced by your project.");
            }

            return new DomainEvent
            {
                AggregateRoot = root,
                Body = (IEvent) _dataSerializer.Deserialize(bsonDocument[BodyElement].AsBsonDocument, eventType), //todo
                Meta = new MessageMetadata
                {
                    Timestamp = bsonDocument[$"{MetaElement}"][$"{TimestampElement}"].ToUniversalTime(),
                    SenderId = bsonDocument[$"{MetaElement}"][$"{SenderElement}"].AsString
                }
            };
        }

        public List<DomainEvent> DeserializeMany(IEnumerable<BsonValue> eventArray, string root)
        {
            return eventArray.Select(eventValue => Deserialize(eventValue.AsBsonDocument, root)).ToList();
        }
    }
}