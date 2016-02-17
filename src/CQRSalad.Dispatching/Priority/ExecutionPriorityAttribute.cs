using System;

namespace CQRSalad.Dispatching.Priority
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class ExecutionPriorityAttribute : Attribute
    {
        public ExecutionPriority ExecutionPriority { get; private set; }

        public ExecutionPriorityAttribute(ExecutionPriority executionPriority)
        {
            ExecutionPriority = executionPriority;
        }
    }
}