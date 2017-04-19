using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public class InMemoryEventStore : IEventStoreAdapter
    {
        private readonly ConcurrentDictionary<string, List<IEvent>> _streams = new ConcurrentDictionary<string, List<IEvent>>();
        private static readonly List<IEvent> EmptyList = new List<IEvent>();
        
        public async Task<IEnumerable<IEvent>> GetStreamAsync(string streamId)
        {
            Argument.IsNotNull(streamId, nameof(streamId));
            return await Task.FromResult(SafePick(streamId));
        }

        public async Task<IEnumerable<IEvent>> GetStreamAsync(string streamId, int fromVersion, int toVersion = -1)
        {
            Argument.IsNotNull(streamId, nameof(streamId));
            Argument.NotNegative(fromVersion, nameof(fromVersion));

            int takeCount = toVersion > 0 ? toVersion : _streams[streamId].Count;
            List<IEvent> streamPart = SafePick(streamId).Skip(fromVersion - 1).Take(takeCount).ToList();
            return await Task.FromResult(streamPart);
        }

        public async Task AppendEventsAsync(string streamId, IEnumerable<IEvent> events, StreamMetadata streamMetadata)
        {
            Argument.ElementsNotNull(events);
            var stream = _streams.GetOrAdd(streamId, key => new List<IEvent>());
            stream.AddRange(events);
            await Task.CompletedTask;
        }

        private IEnumerable<IEvent> SafePick(string streamId)
        {
            return _streams.ContainsKey(streamId) ? _streams[streamId] : EmptyList;
        }
    }
}