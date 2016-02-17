using System;

namespace CQRSalad.EventSourcing.Testing.Exceptions
{
    public sealed class UnexpectedEventException : AggregateTestException
    {
        public UnexpectedEventException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}