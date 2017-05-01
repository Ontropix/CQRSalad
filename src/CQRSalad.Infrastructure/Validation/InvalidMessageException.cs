using System;

namespace CQRSalad.Infrastructure.Validation
{
    public class InvalidMessageException<TMessage> : ApplicationException
    {
        public TMessage MessageInstance { get; set; }

        public InvalidMessageException(string errorMessage, TMessage messageInstance)
            : base(errorMessage)
        {
            MessageInstance = messageInstance;
        }
    }
}