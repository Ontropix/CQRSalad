using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSalad.EventStore.Core
{
    public interface IEventStore
    {
        /// <summary>
        /// Gets all commits
        /// </summary>
        //List<Event> GetAllEvents();
        
        
        /// <summary>
        /// Gets stream of commits
        /// </summary>
        Task<List<DomainEvent>> GetStreamAsync(string streamId);

        /// <summary>
        /// Gets stream of commits
        /// </summary>
        Task<List<DomainEvent>> GetStreamPartAsync(string streamId, int fromVersion, int toVersion = -1);

        /// <summary>
        /// Append event to stream
        /// </summary>
        Task AppendAsync(string streamId, DomainEvent @event);

        /// <summary>
        /// Append several events to stream
        /// </summary>
        Task AppendManyAsync(string streamId, List<DomainEvent> events);
        
        Task<int> CountStreamAsync(string streamId);
    }
}
