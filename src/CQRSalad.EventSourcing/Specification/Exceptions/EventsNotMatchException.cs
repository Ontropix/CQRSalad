using System;

namespace CQRSalad.EventSourcing.Testing.Exceptions
{
    public sealed class EventsNotMatchException : ApplicationException
    {
        public EventsNotMatchException(string message, Exception innerException = null) : 
            base(message, innerException)
        {
        }
    }
}