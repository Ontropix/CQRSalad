using System;

namespace CQRSalad.Dispatching.Priority
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DispatchingPriorityAttribute : Attribute
    {
        public DispatchingPriority Priority { get; private set; }

        public DispatchingPriorityAttribute(DispatchingPriority priority)
        {
            Priority = priority;
        }
    }
}