using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public interface IEventStoreAdapter
    {
        /// <summary>
        /// Gets the stream
        /// </summary>
        Task<EventStream> GetStreamAsync(string streamId);

        /// <summary>
        /// Gets the stream part
        /// </summary>
        Task<EventStream> GetStreamAsync(string streamId, int fromVersion, int toVersion = -1); //todo skip-take?

        /// <summary>
        /// Append several events to the stream
        /// </summary>
        Task AppendEventsAsync(string streamId, IEnumerable<IEvent> events, int expectedVersion, bool isFinalized = false);
    }
}
