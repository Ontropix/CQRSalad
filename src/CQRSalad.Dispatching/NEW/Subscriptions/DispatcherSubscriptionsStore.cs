using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using CQRSalad.Dispatching.NEW.Descriptors;
using CQRSalad.Dispatching.NEW.Priority;

namespace CQRSalad.Dispatching.NEW.Subscriptions
{
    public class DispatcherSubscriptionsManager
    {
        private readonly IDispatcherHandlerDescriptorsBuilder _handlerDescriptorsBuilder;
        private readonly IDispatcherHandlerActionDescriptorsBuilder _actionDescriptorsBuilder;

        public DispatcherSubscriptionsManager(IDispatcherHandlerDescriptorsBuilder handlerDescriptorsBuilder, IDispatcherHandlerActionDescriptorsBuilder actionDescriptorsBuilder)
        {
            _handlerDescriptorsBuilder = handlerDescriptorsBuilder;
            _actionDescriptorsBuilder = actionDescriptorsBuilder;
        }

        public DispatcherSubscriptionsStore Subscribe()
        {
            var store = new DispatcherSubscriptionsStore();

            IEnumerable<HandlerDescriptor> handlerDescriptors = _handlerDescriptorsBuilder.CreateHandlerDescriptors(null); // TODO~!!!!!

            foreach (HandlerDescriptor handlerDescriptor in handlerDescriptors)
            {
                var actionDescriptors = _actionDescriptorsBuilder.CreateActionDescriptors(handlerDescriptor);
                foreach (ActionDescriptor actionDescriptor in actionDescriptors)
                {
                    store.SubscribeAction(
                        new DispatcherSubscription(
                            handlerDescriptor.HandlerType, 
                            actionDescriptor.HandlerAction, 
                            actionDescriptor.MessageType,
                            actionDescriptor.Priority));
                }
            }

            return store;
        }
    }

    public class DispatcherSubscription
    {
        public DispatcherSubscription(TypeInfo handlerType, MethodInfo action, TypeInfo messageType, DispatchingPriority priority)
        {
            HandlerType = handlerType;
            Priority = priority;
            Action = action;
            MessageType = messageType;
        }

        public TypeInfo HandlerType { get; }
        public MethodInfo Action { get; }
        public TypeInfo MessageType { get; }
        public DispatchingPriority Priority { get; }
    }

    public class DispatcherSubscriptionsStore
    {
        // MessageType - List of Actions
        private readonly ConcurrentDictionary<Type, SortedSet<DispatcherSubscription>> _subscriptions = new ConcurrentDictionary<Type, SortedSet<DispatcherSubscription>>();

        public IEnumerable<DispatcherSubscription> this[Type messageType] => GetMessageSubscriptions(messageType);

        internal DispatcherSubscriptionsStore()
        {
        }

        internal void SubscribeAction(DispatcherSubscription subscription)
        {
            if (!_subscriptions.ContainsKey(subscription.MessageType))
            {
                _subscriptions[subscription.MessageType] =
                    new SortedSet<DispatcherSubscription>(
                        Comparer<DispatcherSubscription>.Create((d1, d2) => d1.Priority.CompareTo(d2.Priority))); //todo static?
            }

            _subscriptions[subscription.MessageType].Add(subscription);
        }

        internal IEnumerable<DispatcherSubscription> GetMessageSubscriptions(Type messageType)
        {
            if (!_subscriptions.ContainsKey(messageType))
            {
                throw new HandlerNotFoundException(messageType);
            }
            return _subscriptions[messageType];
        }
    }
}
