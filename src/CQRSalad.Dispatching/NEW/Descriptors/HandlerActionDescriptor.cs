using System;
using System.Collections.Generic;
using System.Reflection;
using CQRSalad.Dispatching.NEW.Priority;

namespace CQRSalad.Dispatching.NEW.Descriptors
{
    public class HandlerActionDescriptor
    {
        public HandlerActionDescriptor(HandlerDescriptor handlerDescriptor, MethodInfo action, DispatchingPriority priority)
        {
            Argument.IsNotNull(handlerDescriptor, nameof(handlerDescriptor));
            Argument.IsNotNull(action, nameof(action));

            HandlerDescriptor = handlerDescriptor;
            HandlerAction = action;
            MessageType = HandlerAction.GetParameters()[0].ParameterType;
            Priority = priority;
        }

        public HandlerDescriptor HandlerDescriptor { get; }
        public MethodInfo HandlerAction { get; }
        public Type MessageType { get; }
        public DispatchingPriority Priority { get; }
    }

    internal class HandlerActionComparer : IEqualityComparer<HandlerActionDescriptor>
    {
        public bool Equals(HandlerActionDescriptor x, HandlerActionDescriptor y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(HandlerActionDescriptor obj)
        {
            throw new NotImplementedException();
        }
    }
}