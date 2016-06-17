using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.Dispatching.NEW.TypesScanning
{
    public interface IDispatcherTypesProvider
    {
        IEnumerable<TypeInfo> GetTypes();
    }
}