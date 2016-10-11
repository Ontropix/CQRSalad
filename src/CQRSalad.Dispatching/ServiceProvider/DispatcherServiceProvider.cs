using System;
using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.Dispatching.ServiceProvider
{
    public interface IDispatcherServiceProvider
    {
        object GetHandlerInstance(TypeInfo handlerType);
        IEnumerable<object> GetInterceptors();
    }

    public class DefaultDispatcherServiceProvider : IDispatcherServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultDispatcherServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetHandlerInstance(TypeInfo handlerType)
        {
            return _serviceProvider.GetService(handlerType);
        }

        public IEnumerable<object> GetInterceptors()
        {
            throw new NotImplementedException();
        }
    }
}