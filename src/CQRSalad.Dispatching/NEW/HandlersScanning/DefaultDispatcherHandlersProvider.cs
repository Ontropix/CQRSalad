using System.Collections.Generic;
using System.Reflection;
using CQRSalad.Dispatching.NEW.TypesScanning;

namespace CQRSalad.Dispatching.NEW.HandlersScanning
{
    public class DefaultDispatcherHandlersProvider : IDispatcherHandlersProvider
    {
        public IEnumerable<TypeInfo> GetHandlerTypes(IDispatcherTypesProvider typesProvider)
        {
            var types = typesProvider.GetTypes();

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