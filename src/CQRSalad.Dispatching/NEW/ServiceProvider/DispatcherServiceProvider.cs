using System;
using System.Collections.Generic;
using CQRSalad.Dispatching.NEW.Descriptors;

namespace CQRSalad.Dispatching.NEW.Core
{
    public interface IDispatcherServiceProvider
    {
        object GetHandlerInstance(HandlerDescriptor handlerDescriptor);
        IEnumerable<object> GetInterceptors();
    }

    public class DefaultDispatcherServiceProvider : IDispatcherServiceProvider
    {
        private readonly IServiceProvider _serviceProvider;

        public DefaultDispatcherServiceProvider(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public object GetHandlerInstance(HandlerDescriptor handlerDescriptor)
        {
            return _serviceProvider.GetService(handlerDescriptor.HandlerType);
        }

        public IEnumerable<object> GetInterceptors()
        {
            throw new NotImplementedException();
        }
    }
}