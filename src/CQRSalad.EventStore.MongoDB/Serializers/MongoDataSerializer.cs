using System;
using MongoDB.Bson;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;

namespace CQRSalad.EventStore.MongoDB.Serializers
{
    internal class MongoDataSerializer
    {
        public Object Deserialize(BsonDocument doc, Type type)
        {
            return BsonSerializer.Deserialize(doc, type);
        }

        public T Deserialize<T>(BsonDocument doc)
        {
            return BsonSerializer.Deserialize<T>(doc);
        }

        public BsonDocument Serialize(Object obj)
        {
            var data = new BsonDocument();
            var writer = new BsonDocumentWriter(data);
            BsonSerializer.Serialize(writer, obj.GetType(), obj);

            data.Remove("_t");
            return data;
        }
    }
}
