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

        public IDispatcherSubscriptionsStore Subscribe()
        {
            var store = new DefaultSubscriptionsStore();

            IEnumerable<TypeInfo> handlerTypes = _handlersProvider.GetHandlerTypes();
            IEnumerable<HandlerDescriptor> handlerDescriptors = _handlerDescriptorsBuilder.CreateHandlerDescriptors(handlerTypes);

            foreach (HandlerDescriptor handlerDescriptor in handlerDescriptors)
            {
                var actionDescriptors = _actionDescriptorsBuilder.CreateActionDescriptors(handlerDescriptor);
                foreach (ActionDescriptor actionDescriptor in actionDescriptors)
                {
                    store.Add(new DispatcherSubscription
                    {
                        MessageType = actionDescriptor.MessageType,
                        HandlerType = handlerDescriptor.HandlerType,
                        Action = actionDescriptor.HandlerAction,
                        Priority =
                            actionDescriptor.Priority != DispatchingPriority.Unspecified
                                ? actionDescriptor.Priority
                                : handlerDescriptor.Priority //todo move to priority resolver
                    });
                }
            }

            return store;
        }
    }
}