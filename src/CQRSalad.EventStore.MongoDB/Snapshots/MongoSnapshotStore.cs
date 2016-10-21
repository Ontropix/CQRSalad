using System.Threading.Tasks;
using CQRSalad.EventSourcing;
using CQRSalad.EventStore.Core;
using MongoDB.Bson;
using MongoDB.Driver;

namespace CQRSalad.EventStore.MongoDB
{
    public class MongoSnapshotsOptions
    {
        public string CollectionName { get; set; }
    }

    public class MongoSnapshotStore : ISnapshotStore
    {
        private readonly IMongoDatabase _mongoDatabase;
        private readonly MongoSnapshotsOptions _options;
        private readonly MongoSnapshotSerializer _snapshotSerializer;
        private readonly UpdateOptions MockUpsert = new UpdateOptions() { IsUpsert = true};

        public MongoSnapshotStore(IMongoDatabase mongoDatabase, MongoSnapshotsOptions options)
        {
            _mongoDatabase = mongoDatabase;
            _options = options;
            _snapshotSerializer = new MongoSnapshotSerializer();
        }
        
        private IMongoCollection<BsonDocument> GetCollection()
        {
            return _mongoDatabase.GetCollection<BsonDocument>(_options.CollectionName);
        }

        public async Task<AggregateSnapshot> LoadSnapshot(string aggregateId)
        {
            var snapshot = await GetCollection().Find(_snapshotSerializer.GetIdFilter(aggregateId)).FirstOrDefaultAsync();
            return snapshot != null ? _snapshotSerializer.Deserialize(snapshot) : null;
        }

        public async Task SaveSnapshot(AggregateSnapshot snapshot)
        {
            await GetCollection().ReplaceOneAsync(_snapshotSerializer.GetIdFilter(snapshot.AggregateId), _snapshotSerializer.Serialize(snapshot), MockUpsert);
        }
    }
}