using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSalad.EventStore.Core
{
    public class InMemoryEventStore : IEventStore
    {
        private readonly Dictionary<string, List<DomainEvent>> _streams = new Dictionary<string, List<DomainEvent>>();
        private static readonly List<DomainEvent> EmptyList = new List<DomainEvent>();

        public async Task<List<DomainEvent>> GetStreamAsync(string aggregateId)
        {
            Argument.IsNotNull(aggregateId, nameof(aggregateId));
            return await Task.FromResult(SafePick(aggregateId)); //todo everywhere
        }

        public async Task<List<DomainEvent>> GetStreamPartAsync(string aggregateId, int fromVersion, int toVersion = -1)
        {
            Argument.IsNotNull(aggregateId, nameof(aggregateId));
            Argument.NotNegative(fromVersion, nameof(fromVersion));

            int takeCount = toVersion > 0 ? toVersion : _streams[aggregateId].Count;
            List<DomainEvent> streamPart = _streams[aggregateId].Skip(fromVersion - 1).Take(takeCount).ToList();
            return await Task.FromResult(streamPart);
        }

        public async Task AppendAsync(string streamId, DomainEvent @event)
        {
            Argument.IsNotNull(@event, nameof(@event));

            if (!_streams.ContainsKey(streamId))
            {
                _streams[streamId] = new List<DomainEvent>();
            }
            _streams[streamId].Add(@event);
            await MockAsync();
        }

        public async Task AppendManyAsync(string streamId, List<DomainEvent> events)
        {
            Argument.ElementsNotNull(events);
            if (!_streams.ContainsKey(streamId))
            {
                _streams[streamId] = new List<DomainEvent>();
            }
            _streams[streamId].AddRange(events);
            await MockAsync();
        }

        public async Task<int> CountStreamAsync(string aggregateId)
        {
            return await Task.FromResult(_streams[aggregateId].Count);
        }

        private async Task MockAsync()
        {
            await Task.FromResult(false);
        }

        private List<DomainEvent> SafePick(string streamId)
        {
            return _streams.ContainsKey(streamId) ? _streams[streamId] : EmptyList;
        }
    }
}