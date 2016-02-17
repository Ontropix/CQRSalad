using System;
using System.Reflection;
using CQRSalad.Dispatching.Priority;

namespace CQRSalad.Dispatching.Payload
{
    internal sealed class MessageSubscription
    {
        public Type HandlerType { get; set; }
        public Type MessageType { get; set; }
        public Type ReturnType { get; set; }
        public MethodInfo HandlerMethod { get; set; }
        public ExecutionPriority Priority { get; set; }
    }
}