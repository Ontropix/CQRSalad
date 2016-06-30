using System.Collections.Generic;

namespace CQRSalad.Dispatching.NEW.Descriptors
{
    public interface IDispatcherHandlerActionDescriptorsBuilder
    {
        IEnumerable<ActionDescriptor> CreateActionDescriptors(HandlerDescriptor handlerDescriptor);
    }
}