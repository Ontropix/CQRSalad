using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSalad.EventSourcing;

namespace CQRSalad.Infrastructure
{
    public class InMemoryEventStore : IEventStoreAdapter
    {
        private readonly ConcurrentDictionary<string, EventStream> _streams = new ConcurrentDictionary<string, EventStream>();
        
        public async Task<EventStream> GetStreamAsync(string streamId)
        {
            Argument.IsNotNull(streamId, nameof(streamId));

            var stream = _streams.GetOrAdd(streamId, GetEmptyStream);
            return await Task.FromResult(stream);
        }

        public int FirstEventIndex => 1;

        public async Task<EventStream> GetStreamAsync(string streamId, int fromVersion, int toVersion = -1)
        {
            Argument.IsNotNull(streamId, nameof(streamId));
            Argument.NotNegative(fromVersion, nameof(fromVersion));

            var stream = _streams.GetOrAdd(streamId, GetEmptyStream);

            int takeCount = toVersion >= FirstEventIndex ? toVersion : stream.Events.Count();
            var slice = stream.Events.Skip(fromVersion - FirstEventIndex).Take(takeCount).ToList();
            
            return await Task.FromResult(new EventStream
            {
                Id = streamId,
                Version = stream.Version,
                Events = slice,
                IsClosed = stream.IsClosed
            });
        }

        public async Task AppendEventsAsync(string streamId, IEnumerable<object> events, int expectedVersion, bool isEndOfStream)
        {
            Argument.IsNotNull(streamId, nameof(streamId));
            Argument.ElementsNotNull(events);

            var stream = _streams.GetOrAdd(streamId, GetEmptyStream);

            stream.Events.ToList().AddRange(events);
            stream.Version++;
            stream.IsClosed = isEndOfStream;

            await Task.CompletedTask;
        }

        private static EventStream GetEmptyStream(string key)
        {
            return new EventStream
            {
                Id = key,
                Version = 0,
                Events = new List<object>(),
                IsClosed = false
            };
        }
    }
}