using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public interface IEventStoreAdapter
    {
        /// <summary>
        /// Gets first event position in the stream
        /// </summary>
        int FirstEventIndex { get; }

        /// <summary>
        /// Gets the stream
        /// </summary>
        Task<EventStream> GetStreamAsync(string streamId);

        /// <summary>
        /// Gets the stream slice
        /// </summary>
        Task<EventStream> GetStreamAsync(string streamId, int fromVersion, int toVersion = -1);

        /// <summary>
        /// Append several events to the stream
        /// </summary>
        Task AppendEventsAsync(string streamId, IEnumerable<object> events, int expectedVersion, bool isEndOfStream);
    }
}
