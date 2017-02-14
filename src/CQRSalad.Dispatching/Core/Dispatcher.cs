using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    public class Dispatcher
    {
        private IServiceProvider ServiceProvider { get; }
        private readonly List<Type> _interceptorsTypes;
        private bool ThrowIfMultipleSendingHandlersFound { get; }

        private readonly DispatcherExecutorsManager _executorsManager;
        private SubscriptionsStore Subscriptions { get; }
        
        public static Dispatcher Create(Action<DispatcherConfig> configurator)
        {
            var config = new DispatcherConfig();
            configurator(config);
            Dispatcher instance = Create(config);
            return instance;
        }

        public static Dispatcher Create(DispatcherConfig config)
        {
            return new Dispatcher(
                config.ServiceProvider,
                config.Interceptors,
                config.ThrowIfMultipleSendingHandlersFound);
        }

        private Dispatcher(
            IServiceProvider serviceProvider,
            List<Type> interceptorsTypes, 
            bool throwIfMultipleSendingHandlersFound)
        {
            Argument.IsNotNull(serviceProvider, nameof(serviceProvider));

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

            ServiceProvider = serviceProvider;
            _interceptorsTypes = interceptorsTypes;
            ThrowIfMultipleSendingHandlersFound = throwIfMultipleSendingHandlersFound;
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