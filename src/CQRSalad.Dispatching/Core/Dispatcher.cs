using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSalad.Dispatching.Context;
using CQRSalad.Dispatching.Interceptors;
using CQRSalad.Dispatching.Subscriptions;

namespace CQRSalad.Dispatching.Core
{
    public class Dispatcher : IMessageDispatcher
    {
        private readonly DispatcherSubscriptionsStore _subscriptionsStore;
        private readonly DispatcherExecutorsManager _executorsManager;
        private readonly IServiceProvider _serviceProvider;
        private readonly List<Type> _interceptorsTypes;

        private Dispatcher(
            IServiceProvider serviceProvider,
            DispatcherSubscriptionsStore subscriptionsStore,
            DispatcherExecutorsManager executorsManager, 
            List<Type> interceptorsTypes)
        {
            Argument.IsNotNull(serviceProvider, nameof(serviceProvider));
            Argument.IsNotNull(subscriptionsStore, nameof(subscriptionsStore));
            Argument.IsNotNull(executorsManager, nameof(executorsManager));

            if (interceptorsTypes != null && interceptorsTypes.Count > 0)
            {
                foreach (Type interceptorType in interceptorsTypes)
                {
                    if (!typeof(IContextInterceptor).IsAssignableFrom(interceptorType))
                    {
                        throw new ArgumentException($"Interceptor {interceptorType.FullName} must implement IContextInterceptor");
                    }
                }
            }

            _subscriptionsStore = subscriptionsStore;
            _serviceProvider = serviceProvider;
            _executorsManager = executorsManager;
            _interceptorsTypes = interceptorsTypes;
        }

        public static Dispatcher Create(DispatcherConfiguration configuration)
        {
            return new Dispatcher(
                configuration.ServiceProvider,
                configuration.SubscriptionsStore,
                configuration.ExecutorManager,
                configuration.Interceptors);
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
            object handlerInstance = _serviceProvider.GetMessageHandler(subscription.HandlerType);
            ContextExecutor executor = _executorsManager.GetExecutor(subscription);

            var context = new DispatchingContext(handlerInstance, messageInstance);

            List<IContextInterceptor> interceptors = _serviceProvider.GetInterceptors(_interceptorsTypes);
            interceptors.ForEach(async interceptor => await interceptor.OnExecuting(context));

            try
            {
                await executor.Execute(context);
            }
            catch (Exception exception)
            {
                interceptors.ForEach(async interceptor => await interceptor.OnException(context, exception));
                throw;
            }
            interceptors.ForEach(async interceptor => await interceptor.OnExecuted(context));

            return context.Result;
        }
    }
}