using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSalad.Dispatching.Context;
using CQRSalad.Dispatching.ServiceProvider;
using CQRSalad.Dispatching.Subscriptions;

namespace CQRSalad.Dispatching.Core
{
    public class Dispatcher : IMessageDispatcher
    {
        private readonly DispatcherSubscriptionsStore _subscriptionsStore;
        private readonly DispatcherExecutorsManager _executorsManager;
        private readonly IDispatcherServiceProvider _serviceProvider;

        private Dispatcher(
            DispatcherSubscriptionsStore subscriptionsStore, 
            IDispatcherServiceProvider serviceProvider, 
            DispatcherExecutorsManager executorsManager)
        {
            _subscriptionsStore = subscriptionsStore;
            _serviceProvider = serviceProvider;
            _executorsManager = executorsManager;
        }

        public static Dispatcher Create(DispatcherConfiguration configuration)
        {
            return new Dispatcher(
                configuration.SubscriptionsStore,
                configuration.ServiceProvider,
                configuration.ExecutorManager);
        }

        public static Dispatcher Create(Action<DispatcherConfiguration> configurator)
        {
            var config = new DispatcherConfiguration();
            configurator(config);
            Dispatcher instance = Create(config);
            return instance;
        }

        public async Task PublishAsync<TMessage>(TMessage message)
        {
            List<DispatcherSubscription> subscriptions = _subscriptionsStore[message.GetType()].ToList();
            foreach (DispatcherSubscription subscription in subscriptions)
            {
                await DispatchMessageAsync(message, subscription);
            }
        }

        public async Task<object> SendAsync(object message)
        {
            List<DispatcherSubscription> subscriptions = _subscriptionsStore[message.GetType()].ToList();
            if (subscriptions.Count > 1)
            {
                throw new AmbiguousHandlingException(message);
            }

            return await DispatchMessageAsync(message, subscriptions[0]);
        }

        private async Task<object> DispatchMessageAsync(object messageInstance, DispatcherSubscription subscription)
        {
            object handlerInstance = _serviceProvider.GetHandlerInstance(subscription.HandlerType);
            ContextExecutor executor = _executorsManager.GetExecutor(subscription);

            var context = new DispatchingContext(handlerInstance, messageInstance);
            await executor.Execute(context);
            return context.Result;
            //return _interceptorTypes.Count > 0 ? await InterceptAsync(context) : await context.InvokeAsync();
        }

        //private async Task<object> InterceptAsync(AsyncDispatcherContext context)
        //{
        //    List<IContextInterceptor> interceptors = _serviceProvider.GetInterceptors(_interceptorTypes);

        //    interceptors.ForEach(interceptor => interceptor.OnInvocationStarted(context));
        //    try
        //    {
        //        await context.InvokeAsync();
        //    }
        //    catch (Exception exception)
        //    {
        //        interceptors.ForEach(interceptor => interceptor.OnException(context, exception));
        //        throw;
        //    }
        //    interceptors.ForEach(interceptor => interceptor.OnInvocationFinished(context));

        //    return context.Result;
        //}
    }
}