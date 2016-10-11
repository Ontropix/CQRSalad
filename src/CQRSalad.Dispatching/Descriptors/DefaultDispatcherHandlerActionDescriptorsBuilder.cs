using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSalad.Dispatching.ActionsScanning;
using CQRSalad.Dispatching.Priority;

namespace CQRSalad.Dispatching.Descriptors
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

        public IEnumerable<ActionDescriptor> CreateActionDescriptors(HandlerDescriptor handlerDescriptor)
        {
            IEnumerable<MethodInfo> handlerActions = _handlerActionsProvider.GetHandlerActions(handlerDescriptor.HandlerType);
            IEnumerable<ActionDescriptor> actionDescriptors = handlerActions.Select(
                action => new ActionDescriptor(
                    action, 
                    _priorityProvider.GetActionPriority(action)));

            return actionDescriptors;
        }
    }
}