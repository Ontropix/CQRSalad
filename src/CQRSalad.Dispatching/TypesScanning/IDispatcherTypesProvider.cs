using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.Dispatching.TypesScanning
{
    public interface IDispatcherTypesProvider
    {
        IEnumerable<TypeInfo> GetTypes();
    }
}