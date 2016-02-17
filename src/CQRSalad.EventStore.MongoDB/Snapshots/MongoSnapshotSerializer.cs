using System;
using System.Linq.Expressions;
using System.Runtime.Serialization;
using CQRSalad.EventSourcing;
using CQRSalad.EventStore.MongoDB.Serializers;
using MongoDB.Bson;

namespace CQRSalad.EventStore.MongoDB
{
    internal static class BsonExtensions
    {
        public static Type AsType(this BsonValue bsonValue)
        {
            string typeName = bsonValue.AsString;
            Type type = Type.GetType(typeName);
            if (type == null)
            {
                throw new SerializationException($"Cannot load type: {typeName}. Make sure that assembly containing this type is referenced by your project.");
            }
            return type;
        }
    }

    internal class MongoSnapshotSerializer
    {
        private readonly MongoDataSerializer _dataSerializer;

        private const string AggregateIdElement = "_id";
        private const string AggregateTypeElement = "AggregateType";
        private const string VersionElement = "Version";
        private const string StateElement = "State";
        private const string StateTypeElement = "StateType";
        private const string TimestampElement = "Timestamp";
        

        public MongoSnapshotSerializer()
        {
            _dataSerializer = new MongoDataSerializer();
        }

        public Expression<Func<BsonDocument, bool>> GetIdFilter(string id)
        {
            return document => document[AggregateIdElement] == id;
        }

        public BsonDocument Serialize(AggregateSnapshot snapshot)
        {
            return new BsonDocument()
            {
                { AggregateIdElement, snapshot.AggregateId },
                { AggregateTypeElement, snapshot.AggregateType.AssemblyQualifiedName },
                { VersionElement, snapshot.Version },
                { TimestampElement, snapshot.Timestamp },

                { StateTypeElement, snapshot.State.GetType().AssemblyQualifiedName },
                { StateElement, _dataSerializer.Serialize(snapshot.State) },
            };
        }

        public AggregateSnapshot Deserialize(BsonDocument bsonDocument)
        {
            return new AggregateSnapshot()
            {
                AggregateId = bsonDocument[AggregateIdElement].AsString,
                AggregateType = bsonDocument[AggregateTypeElement].AsType(),
                Version = bsonDocument[VersionElement].AsInt32,
                Timestamp = bsonDocument[TimestampElement].ToUniversalTime(),
                State = _dataSerializer.Deserialize(bsonDocument[StateElement].AsBsonDocument, bsonDocument[StateTypeElement].AsType())
            };
        }
    }
}
