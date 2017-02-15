using System;
using System.Reflection;

namespace CQRSalad.Dispatching
{
    internal class DispatcherSubscription
    {
        public Type MessageType { get; set; }
        public Type HandlerType { get; set; }
        public MessageInvoker Invoker { get; set; }
        public Priority Priority { get; set; }

        //todo override Equals and GetHashCode
    }
}