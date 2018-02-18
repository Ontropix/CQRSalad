using System.Collections.Generic;

namespace CQRSalad.EventSourcing
{
    public class EventStream
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public IEnumerable<object> Events { get; set; } = new List<object>();
        public bool IsClosed { get; set; }
    }
}