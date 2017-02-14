using System;
using System.Reflection;
using CQRSalad.Dispatching.Priority;

namespace CQRSalad.Dispatching
{
    internal class DispatcherSubscription
    {
        public Type MessageType { get; set; }
        public Type HandlerType { get; set; }
        public MethodInfo Action { get; set; }
        public Priority.Priority Priority { get; set; }

        //todo override Equals and GetHashCode
    }
}