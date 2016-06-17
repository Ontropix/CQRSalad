using System.Collections.Generic;
using System.Reflection;
using CQRSalad.Dispatching.NEW.TypesScanning;

namespace CQRSalad.Dispatching.NEW.HandlersScanning
{
    public interface IDispatcherHandlersProvider
    {
        IEnumerable<TypeInfo> GetHandlerTypes(IDispatcherTypesProvider typesProvider);
    }
}
