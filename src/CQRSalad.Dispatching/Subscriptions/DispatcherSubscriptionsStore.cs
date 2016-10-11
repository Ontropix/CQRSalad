using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Reflection;
using CQRSalad.Dispatching.Descriptors;
using CQRSalad.Dispatching.HandlersScanning;
using CQRSalad.Dispatching.Priority;

namespace CQRSalad.Dispatching.Subscriptions
{
    public class DispatcherSubscriptionsManager
    {
        private readonly IDispatcherHandlersProvider _handlersProvider;
        private readonly IDispatcherHandlerDescriptorsBuilder _handlerDescriptorsBuilder;
        private readonly IDispatcherHandlerActionDescriptorsBuilder _actionDescriptorsBuilder;

        public DispatcherSubscriptionsManager(
            IDispatcherHandlersProvider handlersProvider, 
            IDispatcherHandlerDescriptorsBuilder handlerDescriptorsBuilder, 
            IDispatcherHandlerActionDescriptorsBuilder actionDescriptorsBuilder)
        {
            _handlersProvider = handlersProvider;
            _handlerDescriptorsBuilder = handlerDescriptorsBuilder;
            _actionDescriptorsBuilder = actionDescriptorsBuilder;
        }

        public DispatcherSubscriptionsStore Subscribe()
        {
            var store = new DispatcherSubscriptionsStore();

            IEnumerable<TypeInfo> handlerTypes = _handlersProvider.GetHandlerTypes();
            IEnumerable<HandlerDescriptor> handlerDescriptors = _handlerDescriptorsBuilder.CreateHandlerDescriptors(handlerTypes);

            foreach (HandlerDescriptor handlerDescriptor in handlerDescriptors)
            {
                var actionDescriptors = _actionDescriptorsBuilder.CreateActionDescriptors(handlerDescriptor);
                foreach (ActionDescriptor actionDescriptor in actionDescriptors)
                {
                    store.Add(
                        new DispatcherSubscription(
                            handlerDescriptor.HandlerType,
                            actionDescriptor.HandlerAction,
                            actionDescriptor.MessageType,
                            actionDescriptor.Priority != DispatchingPriority.Unspecified ? actionDescriptor.Priority : handlerDescriptor.Priority)); //todo move to priority resolver
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

        public IEnumerable<DispatcherSubscription> this[Type messageType] => Get(messageType);

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

        public bool Remove(DispatcherSubscription subscription)
        {
            if (Exists(subscription))
            {
                return true;
            }

            return false;
        }

        public bool Exists(DispatcherSubscription subscription)
        {
            return false;
        }
    }
}
