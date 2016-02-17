using System;

namespace CQRSalad.EventSourcing.Testing.Exceptions
{
    public sealed class EventsNotMatchException : AggregateTestException
    {
        public EventsNotMatchException(string message, Exception innerException = null) : 
            base(message, innerException)
        {
        }
    }
}