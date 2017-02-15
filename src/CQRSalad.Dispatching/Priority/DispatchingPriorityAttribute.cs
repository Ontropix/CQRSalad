using System;

namespace CQRSalad.Dispatching
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class DispatchingPriorityAttribute : Attribute
    {
        public Priority Priority { get; private set; }

        public DispatchingPriorityAttribute(Priority priority)
        {
            Priority = priority;
        }
    }
}