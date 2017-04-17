using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSalad.EventSourcing;
using CQRSalad.EventStore.Core;
using MongoDB.Driver;

namespace CQRSalad.EventStore.MongoDB
{
    public class HeadBasedMongoEventStore : IEventStoreAdapter
    {
        public HeadBasedMongoEventStore(IMongoDatabase mongoDatabase, EventStoreSettings settings)
        {
            //_eventSerializer = new DomainEventSerializer();
            //_mongoDatabase = mongoDatabase;
            //_settings = settings;
            //Streams = mongoDatabase.GetCollection<EventStream>(settings.StreamsesCollectionName);

            //_settings = settings;
            //_updater = Builders<EventStream>.Update;
        }

        public Task<IEnumerable<IEvent>> GetStreamAsync(string streamId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<IEvent>> GetStreamAsync(string streamId, int fromVersion, int toVersion = -1)
        {
            throw new NotImplementedException();
        }

        public Task AppendEventsAsync(string streamId, IEnumerable<IEvent> events, StreamMetadata streamMetadata)
        {
            throw new NotImplementedException();
        }
    }
}
