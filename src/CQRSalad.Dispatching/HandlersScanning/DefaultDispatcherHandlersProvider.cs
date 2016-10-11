using System.Collections.Generic;
using System.Reflection;
using CQRSalad.Dispatching.TypesScanning;

namespace CQRSalad.Dispatching.HandlersScanning
{
    public class DefaultDispatcherHandlersProvider : IDispatcherHandlersProvider
    {
        private readonly IDispatcherTypesProvider _typesProvider;

        public DefaultDispatcherHandlersProvider(IDispatcherTypesProvider typesProvider)
        {
            _typesProvider = typesProvider;
        }

        public IEnumerable<TypeInfo> GetHandlerTypes()
        {
            var types = _typesProvider.GetTypes();

            var handlerTypes = new HashSet<TypeInfo>();
            foreach (TypeInfo typeInfo in types)
            {
                if (IsDispatcherHandler(typeInfo) && !handlerTypes.Contains(typeInfo))
                {
                    handlerTypes.Add(typeInfo);
                }
            }
            
            return handlerTypes;
        }

        private bool IsDispatcherHandler(TypeInfo typeInfo)
        {
            return typeInfo.IsDefined(typeof(DispatcherHandlerAttribute))
                   && typeInfo.IsClass
                   && typeInfo.IsPublic
                   && !typeInfo.IsAbstract
                   && !typeInfo.IsGenericTypeDefinition
                   && !typeInfo.ContainsGenericParameters;
        }
    }
}