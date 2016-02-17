using System;
using System.Collections.Generic;
using System.Linq;

namespace CQRSalad.Dispatching
{
    internal static class ServiceProviderExtensions
    {
        internal static object GetMessageHandler(this IServiceProvider serviceProvider, Type handlerType)
        {
            object handlerInstance;
            try
            {
                handlerInstance = serviceProvider.GetService(handlerType);
            }
            catch (Exception exception)
            {
                throw new ServiceProviderException(exception);
            }

            if (handlerInstance == null)
            {
                throw new ServiceProviderException(String.Format("ServiceProvider has returned null for type '{0}'.", handlerType));
            }

            return handlerInstance;
        }

        internal static List<IContextInterceptor> GetInterceptors(this IServiceProvider serviceProvider, IEnumerable<Type> interceptorsTypes)
        {
            List<IContextInterceptor> interceptorsInstances;
            try
            {
                interceptorsInstances = interceptorsTypes.Select(serviceProvider.GetService)
                                                         .Cast<IContextInterceptor>()
                                                         .ToList();
            }
            catch (Exception exception)
            {
                throw new ServiceProviderException("An exception ocurred during creating interceptors. See the inner exception for details.", exception);
            }

            if (!interceptorsInstances.Any() || interceptorsInstances.Any(x => x == null))
            {
                throw new ServiceProviderException("ServiceProvider couldn't create interceptor.");
            }

            return interceptorsInstances;
        }
    }
}