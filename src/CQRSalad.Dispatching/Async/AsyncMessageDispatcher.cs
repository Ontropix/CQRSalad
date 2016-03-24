using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSalad.Dispatching.Payload;

namespace CQRSalad.Dispatching.Async
{
    public interface IMessageRouter
    {

    }

    public sealed class AsyncMessageDispatcher
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly AsyncHandlersRegistry _registry;
        private ConcurrentBag<Type> _interceptorTypes { get; set; }

        internal AsyncMessageDispatcher(DispatcherConfiguration config)
        {
            _serviceProvider = config.ServiceLocator;
            _interceptorTypes = new ConcurrentBag<Type>(config.Interceptors);
            _registry = new AsyncHandlersRegistry(config.IsHandlingPriorityEnabled);
            _registry.RegisterHandlers(config.ScanningRules);
        }

        public async Task<object> DispatchAsync(object message)
        {
            Argument.IsNotNull(message, "message");

            List<MessageSubscription> subscriptions = _registry.GetSubscriptions(message.GetType());
            if (subscriptions.Count > 1)
            {
                throw new AmbiguousHandlingException(message);
            }

            return await DispatchMessageAsync(message, subscriptions[0]);
        }

        public async Task PublishAsync(object message)
        {
            Argument.IsNotNull(message, "message");

            List<MessageSubscription> subscriptions = _registry.GetSubscriptions(message.GetType());
            foreach (MessageSubscription subscription in subscriptions)
            {
                await DispatchMessageAsync(message, subscription);
            }
        }

        private async Task<object> DispatchMessageAsync(object message, MessageSubscription subscription)
        {
            object handler = _serviceProvider.GetMessageHandler(subscription.HandlerType);
            var context = new AsyncDispatcherContext(handler, subscription.HandlerMethod, message);

            return _interceptorTypes.Count > 0 ? await InterceptAsync(context) : await context.InvokeAsync();
        }

        private async Task<object> InterceptAsync(AsyncDispatcherContext context)
        {
            List<IContextInterceptor> interceptors = _serviceProvider.GetInterceptors(_interceptorTypes);

            interceptors.ForEach(interceptor => interceptor.OnInvocationStarted(context));
            try
            {
                await context.InvokeAsync();
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
