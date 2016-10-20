using System;

namespace CQRSalad.Dispatching
{
    public sealed class HandlerNotFoundException : DispatchingException
    {
        public Type MessageType { get; private set; }

        public HandlerNotFoundException(Type messageType)
            : base($"Handler for message '{messageType.FullName}' was not found.")
        {
            this.MessageType = messageType;
        }
    }
}