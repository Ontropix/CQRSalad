using System;

namespace CQRSalad.EventSourcing.Specification
{
    public sealed class UnexpectedEventException : ApplicationException
    {
        public UnexpectedEventException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}