using System.Reflection;
using CQRSalad.Dispatching.Priority;

namespace CQRSalad.Dispatching.Subscriptions
{
    public class DispatcherSubscription
    {
        public TypeInfo MessageType { get; set; }
        public TypeInfo HandlerType { get; set; }
        public MethodInfo Action { get; set; }
        public DispatchingPriority Priority { get; set; }

        //todo override Equals and GetHashCode
    }
}