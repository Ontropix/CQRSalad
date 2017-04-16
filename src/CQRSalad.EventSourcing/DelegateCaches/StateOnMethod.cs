using System;

namespace CQRSalad.EventSourcing
{
    internal class StateOnMethod
    {
        public Type StateType { get; set; }
        public Type EventType { get; set; }
        public Action<object, object> Invoker { get; set; }
    }
}