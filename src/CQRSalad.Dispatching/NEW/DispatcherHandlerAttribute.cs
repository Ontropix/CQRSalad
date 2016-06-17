using System;

namespace CQRSalad.Dispatching.NEW
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public sealed class DispatcherHandlerAttribute : Attribute
    {
    }
}