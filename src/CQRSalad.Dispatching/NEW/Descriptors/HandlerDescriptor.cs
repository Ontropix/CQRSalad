using System;
using System.Reflection;
using CQRSalad.Dispatching.NEW.Priority;

namespace CQRSalad.Dispatching.NEW.Descriptors
{
    public class HandlerDescriptor
    {
        public HandlerDescriptor(TypeInfo handlerType, DispatchingPriority priority)
        {
            Argument.IsNotNull(handlerType, nameof(handlerType));

            HandlerType = handlerType;
            Priority = priority;
        }

        public TypeInfo HandlerType { get; }
        public DispatchingPriority Priority { get; }
    }
}