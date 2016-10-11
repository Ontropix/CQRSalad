using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSalad.Dispatching.Priority;

namespace CQRSalad.Dispatching.Descriptors
{
    public class DefaultDispatcherHandlerDescriptorsBuilder : IDispatcherHandlerDescriptorsBuilder
    {
        private readonly IDispatcherPriorityProvider _priorityProvider;

        public DefaultDispatcherHandlerDescriptorsBuilder(IDispatcherPriorityProvider priorityProvider)
        {
            _priorityProvider = priorityProvider;
        }

        public IEnumerable<HandlerDescriptor> CreateHandlerDescriptors(IEnumerable<TypeInfo> handlerTypes)
        {
            return handlerTypes.Select(handlerType => new HandlerDescriptor(handlerType, _priorityProvider.GetHandlerPriority(handlerType)));
        }
    }
}