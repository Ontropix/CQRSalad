using System;

namespace CQRSalad.Dispatching
{
    public sealed class HandlerNotFoundException : DispatchingException
    {
        public Type MessageType { get; private set; }

        public HandlerNotFoundException(Type messageType)
            : base(String.Format("Handler for message '{0}' was not found.", messageType.FullName))
        {
            this.MessageType = messageType;
        }
    }
}