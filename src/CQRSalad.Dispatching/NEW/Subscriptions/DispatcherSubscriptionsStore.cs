using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CQRSalad.Dispatching.NEW.Descriptors;

namespace CQRSalad.Dispatching.NEW.Subscriptions
{
    public class DispatcherSubscriptionsManager
    {
        private readonly DispatcherSubscriptionsStore _subscriptionsStore;

        public DispatcherSubscriptionsManager(DispatcherSubscriptionsStore subscriptionsStore)
        {
            _subscriptionsStore = subscriptionsStore;
        }
        
    }

    public class DispatcherSubscriptionsStore
    {
        // MessageType - List of Actions
        private readonly ConcurrentDictionary<Type, SortedSet<HandlerActionDescriptor>> _subscriptions = new ConcurrentDictionary<Type, SortedSet<HandlerActionDescriptor>>();

        internal void SubscribeAction(HandlerActionDescriptor actionDescriptor)
        {
            if (!_subscriptions.ContainsKey(actionDescriptor.MessageType))
            {
                _subscriptions[actionDescriptor.MessageType] =
                    new SortedSet<HandlerActionDescriptor>(
                        Comparer<HandlerActionDescriptor>.Create((d1, d2) => d1.Priority.CompareTo(d2.Priority)));
            }

            _subscriptions[actionDescriptor.MessageType].Add(actionDescriptor);
        }

        internal bool IsActionSubscribed(HandlerActionDescriptor actionDescriptor)
        {
            if (!_subscriptions.ContainsKey(actionDescriptor.MessageType))
            {
                return false;
            }

            return _subscriptions[actionDescriptor.MessageType].Contains(actionDescriptor);
        }

        internal IEnumerable<HandlerActionDescriptor> GetMessageSubscriptions(Type messageType)
        {
            if (!_subscriptions.ContainsKey(messageType))
            {
                throw new HandlerNotFoundException(messageType);
            }
            return _subscriptions[messageType];
        }
    }
}
