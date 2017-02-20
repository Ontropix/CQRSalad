using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace CQRSalad.Dispatching
{
    internal class SubscriptionsStore
    {
        //NOTE: <MessageType, Subscriptions>
        private readonly ConcurrentDictionary<Type, SortedSet<Subscription>> _store = new ConcurrentDictionary<Type, SortedSet<Subscription>>();

        public void Add(Subscription subscription)
        {
            if (!_store.ContainsKey(subscription.MessageType))
            {
                var comparer = Comparer<Subscription>.Create((d1, d2) => d1.Priority.CompareTo(d2.Priority));
                _store[subscription.MessageType] = new SortedSet<Subscription>(comparer);
            }

            _store[subscription.MessageType].Add(subscription);
        }

        public IList<Subscription> Get(Type messageType)
        {
            if (!_store.ContainsKey(messageType))
            {
                throw new HandlerNotFoundException(messageType);
            }
            return _store[messageType].ToList();
        }
    }
}