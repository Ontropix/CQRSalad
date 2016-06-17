using System.Collections.Generic;

namespace CQRSalad.Dispatching.NEW.Descriptors
{
    public interface IDispatcherHandlerActionDescriptorsBuilder
    {
        IEnumerable<HandlerActionDescriptor> CreateHandlerActionDescriptors(HandlerDescriptor handlerDescriptor);
    }
}