using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSalad.EventStore.Core;
using MongoDB.Driver;

namespace CQRSalad.EventStore.MongoDB
{
    public class HeadBasedMongoEventStore : IEventStore
    {
        public HeadBasedMongoEventStore(IMongoDatabase mongoDatabase, EventStoreSettings settings)
        {
            _eventSerializer = new DomainEventSerializer();
            _mongoDatabase = mongoDatabase;
            _settings = settings;
            Streams = mongoDatabase.GetCollection<EventStream>(settings.StreamsesCollectionName);

            _settings = settings;
            _updater = Builders<EventStream>.Update;
        }

        public Task<List<DomainEvent>> GetStreamAsync(string streamId)
        {
            throw new NotImplementedException();
        }

        public Task<List<DomainEvent>> GetStreamPartAsync(string streamId, int fromVersion, int toVersion = -1)
        {
            throw new NotImplementedException();
        }

        public Task AppendAsync(string streamId, DomainEvent @event)
        {
            throw new NotImplementedException();
        }

        public Task AppendManyAsync(string streamId, List<DomainEvent> events)
        {
            throw new NotImplementedException();
        }

        public Task<int> CountStreamAsync(string streamId)
        {
            throw new NotImplementedException();
        }
    }
}
