using System;
using System.Collections.Generic;
using System.Reflection;
using CQRSalad.Dispatching.Priority;

namespace CQRSalad.Dispatching.Descriptors
{
    public class ActionDescriptor
    {
        public ActionDescriptor(MethodInfo action, DispatchingPriority priority)
        {
            Argument.IsNotNull(action, nameof(action));

            HandlerAction = action;
            MessageType = HandlerAction.GetParameters()[0].ParameterType.GetTypeInfo();
            Priority = priority;
        }

        public MethodInfo HandlerAction { get; }
        public TypeInfo MessageType { get; }
        public DispatchingPriority Priority { get; }
    }

    internal class HandlerActionComparer : IEqualityComparer<ActionDescriptor>
    {
        public bool Equals(ActionDescriptor x, ActionDescriptor y)
        {
            throw new NotImplementedException();
        }

        public int GetHashCode(ActionDescriptor obj)
        {
            throw new NotImplementedException();
        }
    }
}