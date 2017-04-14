using System;

namespace CQRSalad.EventStore.Core
{
    public class InvalidEventException : ApplicationException
    {
        public DomainEvent Event { get; set; }

        public InvalidEventException(string message, DomainEvent @event)
            : base(message)
        {
            Event = @event;
        }
    }
}