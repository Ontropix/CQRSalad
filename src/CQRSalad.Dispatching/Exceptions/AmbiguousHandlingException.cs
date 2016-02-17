using System;

namespace CQRSalad.Dispatching
{
    public sealed class AmbiguousHandlingException : ApplicationException
    {
        public object MessageObject { get; private set; }

        public AmbiguousHandlingException(object messageObject)
            : base("Dispatching failed. Message has multiple handlers.")
        {
            MessageObject = messageObject;
        }
    }
}