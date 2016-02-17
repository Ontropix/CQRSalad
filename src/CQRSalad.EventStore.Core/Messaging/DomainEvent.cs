using CQRSalad.Dispatching.Async;

namespace CQRSalad.EventStore.Core
{
    /// <summary>
    /// Domain event
    /// </summary>
    public sealed class DomainEvent
    {
        public string AggregateId { get; set; }
     
        /// <summary>
        /// Aggregate that produced this event
        /// </summary>
        public string AggregateRoot { get; set; }

        /// <summary>
        /// Event from an aggregate
        /// </summary>
        public IEvent Body { get; set; }

        /// <summary>
        /// Metadata of event
        /// </summary>
        public MessageMetadata Meta { get; set; }
    }
    
}