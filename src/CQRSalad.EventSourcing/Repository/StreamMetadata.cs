using System;
using System.Collections.Generic;

namespace CQRSalad.EventSourcing
{
    public class EventStream
    {
        public string StreamId { get; set; }
        public IEnumerable<IEvent> Events { get; } = new List<IEvent>();
        public int Version { get; set; }
        public StreamMetadata Meta { get; set; }
    }

    public class StreamMetadata
    {
        /// <summary>
        /// Aggregate Id
        /// </summary>
        public string AggregateId { get; set; }

        /// <summary>
        /// Aggregate that produced this event
        /// </summary>
        public Type AggregateType { get; set; }
    }
}