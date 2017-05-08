using System.Collections.Generic;

namespace CQRSalad.EventSourcing
{
    public enum RootStatus
    {
        New = 0,
        Alive = 3,
        Archived = 10
    }

    public interface IAggregateRoot
    {
        string Id { get; set; }

        int Version { get; set; }

        RootStatus Status { get; set; }

        List<IEvent> Changes { get; }

        object State { get; set; }
    }
}