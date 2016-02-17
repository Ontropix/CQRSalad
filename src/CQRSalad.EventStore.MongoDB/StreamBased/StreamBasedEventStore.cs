using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSalad.EventStore.Core;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;

namespace CQRSalad.EventStore.MongoDB
{
    internal class EventStream
    {
        [BsonId]
        //public ObjectId Id { get; set; }
        public string Id { get; set; }

        public string Root { get; set; }
        public int Version { get; set; }
        public BsonArray Events { get; set; }
    }
    
    public class StreamBasedEventStore : IEventStore
    {
        private static readonly List<DomainEvent> EmtpyList = new List<DomainEvent>();
        private static readonly UpdateOptions MockUpsert = new UpdateOptions() { IsUpsert = true };

        private readonly DomainEventSerializer _eventSerializer;
        private readonly IMongoDatabase _mongoDatabase;
        private readonly EventStoreSettings _settings;

        private readonly UpdateDefinitionBuilder<EventStream> _updater;

        private IMongoCollection<EventStream> Streams { get; }

        public StreamBasedEventStore(IMongoDatabase mongoDatabase, EventStoreSettings settings)
        {
            _eventSerializer = new DomainEventSerializer();
            _mongoDatabase = mongoDatabase;
            _settings = settings;
            Streams = mongoDatabase.GetCollection<EventStream>(settings.StreamsesCollectionName);

            _settings = settings;
            _updater = Builders<EventStream>.Update;
        }

        public async Task<List<DomainEvent>> GetStreamAsync(string streamId)
        {
            List<DomainEvent> events = await Streams.Find(stream => stream.Id == streamId)
                                                    .Project(stream => _eventSerializer.DeserializeMany(stream.Events, stream.Root))
                                                    .FirstOrDefaultAsync();
            return events ?? EmtpyList;
        }

        public async Task<List<DomainEvent>> GetStreamPartAsync(string streamId, int fromVersion, int toVersion = -1)
        {
            List<DomainEvent> events = await Streams.Find(stream => stream.Id == streamId)
                                                    .Project(stream => _eventSerializer.DeserializeMany(stream.Events.Skip(fromVersion - 1), stream.Root))
                                                    .FirstOrDefaultAsync();
            return events ?? EmtpyList;
        }

        public async Task AppendAsync(string streamId, DomainEvent @event)
        {
            var update = _updater.SetOnInsert(x => x.Root, @event.AggregateRoot)
                                 .Inc(x => x.Version, 1)
                                 .Push(x => x.Events, _eventSerializer.Serialize(@event));

            await Streams.UpdateOneAsync(stream => stream.Id == streamId, update, MockUpsert);
        }

        public async Task AppendManyAsync(string streamId, List<DomainEvent> events)
        {
            var update = _updater.SetOnInsert(x => x.Root, events.First().AggregateRoot)
                                 .Inc(x => x.Version, events.Count)
                                 .PushEach(x => x.Events, _eventSerializer.SerializeMany(events));

            await Streams.UpdateOneAsync(x => x.Id == streamId, update, MockUpsert);
        }

        public Task<int> CountStreamAsync(string streamId)
        {
            return Streams.Find(stream => stream.Id == streamId).Project(x => x.Version).FirstOrDefaultAsync();
        }
    }
}
