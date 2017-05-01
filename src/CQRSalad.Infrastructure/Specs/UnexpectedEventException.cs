using System;

namespace CQRSalad.Infrastructure
{
    public sealed class UnexpectedEventException : ApplicationException
    {
        public UnexpectedEventException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}