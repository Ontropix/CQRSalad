using System;

namespace CQRSalad.Dispatching
{
    public class DispatchingException : ApplicationException
    {
        public DispatchingException(string message)
            :base(message)
        {
        }

        public DispatchingException(Exception innerException)
            : base("An error occurred during dispatching a message. See the inner exception for details.", innerException)
        {
        }
        
        public DispatchingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}