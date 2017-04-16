using System.Collections.Generic;

namespace CQRSalad.EventSourcing
{
    public interface IAggregateRoot
    {
        string Id { get; set; }

        int Version { get; set; }

        List<IEvent> Changes { get; }

        object State { get; set; }
    }
}