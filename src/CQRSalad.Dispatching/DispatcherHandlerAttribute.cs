using System;

namespace CQRSalad.Dispatching
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DispatcherHandlerAttribute : Attribute
    {
    }
}