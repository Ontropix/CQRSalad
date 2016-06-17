using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.Dispatching.NEW.Descriptors
{
    public interface IDispatcherHandlerDescriptorsBuilder
    {
        IEnumerable<HandlerDescriptor> CreateHandlerDescriptors(IEnumerable<TypeInfo> handlerTypes);
    }
}
    