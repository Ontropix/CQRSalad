using System;

namespace CQRSalad.EventSourcing
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public class AggregateIdAttribute : Attribute
    {
    }
}