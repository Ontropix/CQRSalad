using System.Collections.Generic;

namespace CQRSalad.EventSourcing
{
    public enum AggregateStatus
    {
        New = 0,
        Alive = 3,
        Finalized = 10
    }

    public interface IAggregateRoot
    {
        string Id { get; set; }

        int Version { get; set; }

        AggregateStatus Status { get; set; }

        List<IEvent> Changes { get; }

        object State { get; set; }
    }
}