using System;

namespace CQRSalad.Specifications
{
    public sealed class UnexpectedEventException : ApplicationException
    {
        public UnexpectedEventException(string message, Exception innerException = null) : base(message, innerException)
        {
        }
    }
}