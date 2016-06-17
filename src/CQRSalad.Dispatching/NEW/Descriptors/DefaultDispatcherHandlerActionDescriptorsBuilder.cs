using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSalad.Dispatching.NEW.ActionsScanning;
using CQRSalad.Dispatching.NEW.Priority;

namespace CQRSalad.Dispatching.NEW.Descriptors
{
    public class DefaultDispatcherHandlerActionDescriptorsBuilder : IDispatcherHandlerActionDescriptorsBuilder
    {
        private readonly IDispatcherHandlerActionsProvider _handlerActionsProvider;
        private readonly IDispatcherPriorityProvider _priorityProvider;

        public DefaultDispatcherHandlerActionDescriptorsBuilder(IDispatcherHandlerActionsProvider handlerActionsProvider, IDispatcherPriorityProvider priorityProvider)
        {
            _handlerActionsProvider = handlerActionsProvider;
            _priorityProvider = priorityProvider;
        }

        public IEnumerable<HandlerActionDescriptor> CreateHandlerActionDescriptors(HandlerDescriptor handlerDescriptor)
        {
            IEnumerable<MethodInfo> handlerActions = _handlerActionsProvider.GetHandlerActions(handlerDescriptor.HandlerType);
            IEnumerable<HandlerActionDescriptor> actionDescriptors = handlerActions.Select(action => new HandlerActionDescriptor(handlerDescriptor, action, _priorityProvider.GetActionPriority(action)));
            return actionDescriptors;
        }
    }
}