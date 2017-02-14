using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace CQRSalad.Dispatching
{
    internal class SubscriptionsStore
    {
        // MessageType - List of Actions
        private readonly ConcurrentDictionary<Type, SortedSet<DispatcherSubscription>> _subscriptions = new ConcurrentDictionary<Type, SortedSet<DispatcherSubscription>>();

        public void Add(DispatcherSubscription subscription)
        {
            if (!_subscriptions.ContainsKey(subscription.MessageType))
            {
                _subscriptions[subscription.MessageType] =
                    new SortedSet<DispatcherSubscription>(
                        Comparer<DispatcherSubscription>.Create((d1, d2) => d1.Priority.CompareTo(d2.Priority))); //todo static?
            }

            _subscriptions[subscription.MessageType].Add(subscription);
        }

        public IEnumerable<DispatcherSubscription> Get(Type messageType)
        {
            if (!_subscriptions.ContainsKey(messageType))
            {
                throw new HandlerNotFoundException(messageType);
            }
            return _subscriptions[messageType];
        }
    }
}