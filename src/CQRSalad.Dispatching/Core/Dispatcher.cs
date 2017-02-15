using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    public class Dispatcher
    {
        private IServiceProvider ServiceProvider { get; }
        private readonly IList<Type> _interceptorsTypes;
        private bool ThrowIfMultipleSendingHandlersFound { get; }

        private SubscriptionsStore Subscriptions { get; }

        public static Dispatcher Create(DispatcherConfig config)
        {
            return new Dispatcher(
                config.AssembliesWithHandlers,
                config.ServiceProvider,
                config.Interceptors,
                config.ThrowIfMultipleSendingHandlersFound);
        }

        private Dispatcher(
            IEnumerable<Type> typesToRegister,
            IServiceProvider serviceProvider,
            IList<Type> interceptorsTypes,
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

        private async Task<object> DispatchMessageAsync(object message, DispatcherSubscription subscription)
        {
            object handler = ServiceProvider.GetMessageHandler(subscription.HandlerType);
            var context = new DispatchingContext(handler, message);

            var executor = new DispatcherContextExecutor(subscription.Invoker);

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