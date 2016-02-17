using System;

namespace CQRSalad.Dispatching
{
    public class ServiceProviderException : ApplicationException
    {
        public ServiceProviderException(string message)
            : base(message)
        {
        }

        public ServiceProviderException(Exception innerException)
            : base("An exception ocurred during creating message handler instance. See the inner exception for details.", innerException)
        {
        }

        public ServiceProviderException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}