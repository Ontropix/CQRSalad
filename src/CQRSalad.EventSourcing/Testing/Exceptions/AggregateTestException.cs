using System;

namespace CQRSalad.EventSourcing.Testing.Exceptions
{
    public abstract class AggregateTestException : ApplicationException
    {
        internal AggregateTestException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}