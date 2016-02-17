using System;

namespace CQRSalad.EventSourcing
{
    public sealed class CommandProcessingException : ArgumentException
    {
        public CommandProcessingException(string message, Exception innerException = null)
            : base(message, innerException)
        {
        }
    }
}