using System;

namespace CQRSalad.Dispatching
{
    public sealed class MultipleHandlersException : ApplicationException
    {
        public object MessageObject { get; private set; }

        public MultipleHandlersException(object messageObject)
            : base("Sending message failed. Message has multiple handlers.")
        {
            MessageObject = messageObject;
        }
    }
}