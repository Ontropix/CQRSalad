using CQRSalad.EventStore.Core;

namespace Samples.Domain.Events
{
    public abstract class Event : IEvent
    {
        public string AggregateId { get; set; }
    }
}
