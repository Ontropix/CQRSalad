using System.Collections.Generic;

namespace CQRSalad.EventSourcing
{
    public interface IAggregateRoot
    {
        string Id { get; set; }

        int Version { get; set; }

        List<IEvent> Changes { get; }

        void Reel(List<IEvent> events);

        object State { get; set; }
    }
}