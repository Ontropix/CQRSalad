using System;

namespace CQRSalad.EventSourcing
{
    internal class WhenMethod
    {
        public Type AggregateType { get; set; }
        public Type CommandType { get; set; }
        public Action<object, object> Invoker { get; set; }
        public bool IsCtor { get; set; }
    }
}