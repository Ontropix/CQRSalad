using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public interface IEventStoreAdapter
    {
        /// <summary>
        /// Gets the stream
        /// </summary>
        Task<IEnumerable<IEvent>> GetStreamAsync(string streamId);

        /// <summary>
        /// Gets the stream part
        /// </summary>
        Task<IEnumerable<IEvent>> GetStreamAsync(string streamId, int fromVersion, int toVersion = -1); //todo skip-take?

        /// <summary>
        /// Append several events to the stream
        /// </summary>
        Task AppendEventsAsync(string streamId, IEnumerable<IEvent> events, StreamMetadata streamMetadata);
    }
}
