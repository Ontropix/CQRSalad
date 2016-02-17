using System;

namespace CQRSalad.EventSourcing
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false, Inherited = false)]
    public class AggregateCtorAttribute : Attribute
    {
    }
}
