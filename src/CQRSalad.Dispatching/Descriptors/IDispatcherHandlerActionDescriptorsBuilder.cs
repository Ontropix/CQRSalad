using System.Collections.Generic;

namespace CQRSalad.Dispatching.Descriptors
{
    public interface IDispatcherHandlerActionDescriptorsBuilder
    {
        IEnumerable<ActionDescriptor> CreateActionDescriptors(HandlerDescriptor handlerDescriptor);
    }
}