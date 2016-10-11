using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.Dispatching.HandlersScanning
{
    public interface IDispatcherHandlersProvider
    {
        IEnumerable<TypeInfo> GetHandlerTypes();
    }
}
