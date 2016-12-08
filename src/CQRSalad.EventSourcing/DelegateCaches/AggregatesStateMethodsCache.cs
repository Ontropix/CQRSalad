using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal delegate void EventApplier(object state, object evnt);

    internal class EventApplierSubscription
    {
        public Type StateType { get; set; }
        public Type EventType { get; set; }
        public EventApplier Handler { get; set; }
    }

    internal static class AggregatesStateMethodsCache
    {
        // Event Type - Aggregate State method delegate
        private static readonly ConcurrentDictionary<Type, EventApplierSubscription> _cache = new ConcurrentDictionary<Type, EventApplierSubscription>();

        internal static EventApplierSubscription GetCommandHandler(Type aggregateStateType, Type eventType)
        {
            if (_cache.ContainsKey(eventType))
            {
                return _cache[eventType];
            }

            MethodInfo action = aggregateStateType.FindMethodBySinglePameter(eventType);
            if (action == null)
            {
                throw new CommandProcessingException("Aggregate doesn't handle command.");
            }

            var ctor = action.GetCustomAttribute<AggregateCtorAttribute>(false);
            var handler = DelegateHelper.CreateMessageInvoker<EventApplier>(aggregateStateType, action, eventType);

            var subscription = new EventApplierSubscription
            {
                StateType = aggregateStateType,
                EventType = eventType,
                Handler = handler
            };

            _cache.TryAdd(eventType, subscription);
            return subscription;
        }
    }
}