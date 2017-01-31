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
        private IDispatcherSubscriptionsStore Subscriptions { get; }
        private IServiceProvider ServiceProvider { get; }
        private bool ThrowIfMultipleSendingHandlersFound { get; }

        private readonly DispatcherExecutorsManager _executorsManager;
        private readonly List<Type> _interceptorsTypes;

        private Dispatcher(
            IServiceProvider serviceProvider,
            IDispatcherSubscriptionsStore subscriptionsStore,

            DispatcherExecutorsManager executorsManager, 
            List<Type> interceptorsTypes, 
            bool throwIfMultipleSendingHandlersFound)
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

            Subscriptions = subscriptionsStore;
            ServiceProvider = serviceProvider;
            ThrowIfMultipleSendingHandlersFound = throwIfMultipleSendingHandlersFound;

            _executorsManager = executorsManager;
            _interceptorsTypes = interceptorsTypes;
        }
        
        public static Dispatcher Create(DispatcherConfiguration configuration)
        {
            return new Dispatcher(
                configuration.ServiceProvider,
                configuration.SubscriptionsStore,
                configuration.ExecutorManager,
                configuration.Interceptors,
                configuration.ThrowIfMultipleSendingHandlersFound);
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
            List<DispatcherSubscription> subscriptions = Subscriptions.Get(message.GetType()).ToList();
            foreach (DispatcherSubscription subscription in subscriptions)
            {
                await DispatchMessageAsync(message, subscription);
            }
        }

        public async Task<object> SendAsync(object message)
        {
            List<DispatcherSubscription> subscriptions = Subscriptions.Get(message.GetType()).ToList();
            if (subscriptions.Count > 1 && ThrowIfMultipleSendingHandlersFound)
            {
                throw new MultipleHandlersException(message);
            }

            return await DispatchMessageAsync(message, subscriptions[0]);
        }

        private async Task<object> DispatchMessageAsync(object messageInstance, DispatcherSubscription subscription)
        {
            object handlerInstance = ServiceProvider.GetMessageHandler(subscription.HandlerType);
            var context = new DispatchingContext(handlerInstance, messageInstance);

            IDispatcherContextExecutor executor = _executorsManager.GetExecutor(subscription);
            
            List<IContextInterceptor> interceptors = ServiceProvider.GetInterceptors(_interceptorsTypes);
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