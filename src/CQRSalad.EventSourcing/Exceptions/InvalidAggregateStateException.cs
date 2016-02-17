using System;

namespace CQRSalad.EventSourcing
{
    public sealed class InvalidAggregateStateException : ApplicationException
    {
        public InvalidAggregateStateException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}