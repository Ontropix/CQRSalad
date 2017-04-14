using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSalad.EventSourcing;

namespace CQRSalad.EventStore.Core
{
    public class InMemoryEventStore : IEventStoreAdapter
    {
        private readonly Dictionary<string, List<IEvent>> _streams = new Dictionary<string, List<IEvent>>();
        private static readonly List<IEvent> EmptyList = new List<IEvent>();
        
        public async Task<IEnumerable<IEvent>> GetStreamAsync(string aggregateId)
        {
            Argument.IsNotNull(aggregateId, nameof(aggregateId));
            return await Task.FromResult(SafePick(aggregateId)); //todo everywhere
        }

        public async Task<IEnumerable<IEvent>> GetStreamAsync(string aggregateId, int fromVersion, int toVersion = -1)
        {
            Argument.IsNotNull(aggregateId, nameof(aggregateId));
            Argument.NotNegative(fromVersion, nameof(fromVersion));

            int takeCount = toVersion > 0 ? toVersion : _streams[aggregateId].Count;
            List<IEvent> streamPart = _streams[aggregateId].Skip(fromVersion - 1).Take(takeCount).ToList();
            return await Task.FromResult(streamPart);
        }

        public async Task AppendEventsAsync(string streamId, IEnumerable<IEvent> events, EventMetadata eventMetadata)
        {
            Argument.ElementsNotNull(events);
            if (!_streams.ContainsKey(streamId))
            {
                _streams[streamId] = new List<IEvent>();
            }
            _streams[streamId].AddRange(events);
            await MockAsync();
        }

        private async Task MockAsync()
        {
            await Task.FromResult(false);
        }

        private IEnumerable<IEvent> SafePick(string streamId)
        {
            return _streams.ContainsKey(streamId) ? _streams[streamId] : EmptyList;
        }
    }
}