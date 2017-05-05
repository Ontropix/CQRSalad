using System;
using System.Collections.Generic;

namespace CQRSalad.EventSourcing
{
    public class EventStream
    {
        public string StreamId { get; set; }
        public int Version { get; set; }
        public IEnumerable<IEvent> Events { get; set; } = new List<IEvent>();

        public EventStreamMetadata Metadata { get; set; } //todo
    }

    public class EventStreamMetadata
    {
        public Type AggregateRootType { get; set; } //todo
        public AggregateStatus AggregateStatus { get; set; }
        public DateTime StartedOn { get; set; } //todo
    }
}