using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using CQRSalad.Dispatching.Payload;

namespace CQRSalad.Dispatching
{
    public class MessageDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly HandlersRegistry _registry;
        private ConcurrentBag<Type> _interceptorTypes { get; set; }

        internal MessageDispatcher(DispatcherConfiguration config)
        {
            _serviceProvider = config.ServiceLocator;
            _interceptorTypes = new ConcurrentBag<Type>(config.Interceptors);
            _registry = new HandlersRegistry(config.IsHandlingPriorityEnabled);
            _registry.RegisterHandlers(config.ScanningRules);
        }
        
        public object Dispatch(object message)
        {
            Argument.IsNotNull(message, "message");
            
            try
            {
                List<MessageSubscription> subscriptions = _registry.GetSubscriptions(message.GetType());
                if (subscriptions.Count > 1)
                {
                    throw new AmbiguousHandlingException(message);
                }

                return DispatchMessage(message, subscriptions[0]);
            }
            catch (Exception exception)
            {
                throw new DispatchingException(exception);
            }
        }

        public void Publish(object message)
        {
            Argument.IsNotNull(message, "message");
            
            try
            {
                List<MessageSubscription> subscriptions = _registry.GetSubscriptions(message.GetType());
                foreach (MessageSubscription subscription in subscriptions)
                {
                    DispatchMessage(message, subscription);
                }
            }
            catch (Exception exception)
            {
                throw new DispatchingException(exception);
            }
        }

        private object DispatchMessage(object message, MessageSubscription subscription)
        {
            object handler = _serviceProvider.GetMessageHandler(subscription.HandlerType);
            var context = new DispatcherContext(handler, subscription.HandlerMethod, message);

            object invocationResult = _interceptorTypes.Count > 0 ? Intercept(context) : context.Invoke();
            return invocationResult;
        }

        private object Intercept(DispatcherContext context)
        {
            List<IContextInterceptor> interceptors = _serviceProvider.GetInterceptors(_interceptorTypes);

            interceptors.ForEach(interceptor => interceptor.OnInvocationStarted(context));
            try
            {
                context.Invoke();
            }
            catch (Exception exception)
            {
                interceptors.ForEach(interceptor => interceptor.OnException(context, exception));
                throw;
            }
            interceptors.ForEach(interceptor => interceptor.OnInvocationFinished(context));

            return context.Result;
        }
    }
}
