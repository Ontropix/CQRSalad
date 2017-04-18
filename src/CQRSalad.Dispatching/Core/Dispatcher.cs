using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    public sealed class Dispatcher
    {
        private DispatcherHandlersController Controller { get; }
        private IServiceProvider ServiceProvider { get; }
        private readonly IList<Type> _interceptorsTypes;
        private bool ThrowIfMultipleSendingHandlersFound { get; }

        public static Dispatcher Create(DispatcherConfig config)
        {
            return new Dispatcher(
                new DispatcherHandlersController(config.TypesToRegister, config.HandlersTypesResolver),
                config.ServiceProvider,
                config.Interceptors,
                config.ThrowIfMultipleSendingHandlersFound);
        }

        private Dispatcher(
            DispatcherHandlersController handlersController,
            IServiceProvider serviceProvider,
            IList<Type> interceptorsTypes,
            bool throwIfMultipleSendingHandlersFound)
        {
            Argument.IsNotNull(handlersController, nameof(handlersController));
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

            Controller = handlersController;
            ServiceProvider = serviceProvider;
            _interceptorsTypes = interceptorsTypes;
            ThrowIfMultipleSendingHandlersFound = throwIfMultipleSendingHandlersFound;
        }

        public async Task PublishAsync<TMessage>(TMessage message)
        {
            IList<Subscription> subscriptions = Controller.GetSubscriptionsFor(message.GetType());
            foreach (Subscription subscription in subscriptions)
            {
                await DispatchMessageAsync(message, subscription);
            }
        }

        public async Task<object> SendAsync(object message)
        {
            IList<Subscription> subscriptions = Controller.GetSubscriptionsFor(message.GetType());
            if (subscriptions.Count > 1 && ThrowIfMultipleSendingHandlersFound)
            {
                throw new MultipleHandlersException(message);
            }

            return await DispatchMessageAsync(message, subscriptions[0]);
        }

        private async Task<object> DispatchMessageAsync(object message, Subscription subscription)
        {
            object handler = ServiceProvider.GetMessageHandler(subscription.HandlerType);
            List<IContextInterceptor> interceptors = ServiceProvider.GetInterceptors(_interceptorsTypes);

            var context = new DispatchingContext(handler, message);
            
            interceptors.ForEach(async interceptor => await interceptor.OnExecuting(context));

            try
            {
                await subscription.Invoker.Execute(context);
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