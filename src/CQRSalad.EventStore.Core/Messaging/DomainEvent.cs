namespace CQRSalad.EventStore.Core
{
    /// <summary>
    /// Domain event
    /// </summary>
    public sealed class DomainEvent
    {
        /// <summary>
        /// Id
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// Event from an aggregate
        /// </summary>
        public object Body { get; set; }

        /// <summary>
        /// Metadata of event
        /// </summary>
        public EventMetadata Meta { get; set; }
    }
}
