using System;

namespace CQRSalad.EventStore.Core
{
    public class AggregateSnapshot
    {
        public string AggregateId { get; set; }
        public Type AggregateType { get; set; }
        public int Version { get; set; }
        public object State { get; set; }
        public DateTime Timestamp { get; set; }
    }
}