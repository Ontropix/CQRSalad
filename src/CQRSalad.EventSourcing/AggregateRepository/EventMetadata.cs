using System;

namespace CQRSalad.EventSourcing
{
    public class EventMetadata
    {
        /// <summary>
        /// Aggregate Id
        /// </summary>
        public string AggregateId { get; set; }

        /// <summary>
        /// Aggregate that produced this event
        /// </summary>
        public string AggregateType { get; set; }

        /// <summary>
        /// Time when event commited
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}