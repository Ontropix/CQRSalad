using System;
using System.Threading.Tasks;
using CQRSalad.Dispatching;

namespace CQRSalad.Infrastructure.Interceptors
{
    public abstract class SpecificMessageInterceptor<TMessage> : IContextInterceptor where TMessage : class
    {
        async Task IContextInterceptor.OnExecuting(DispatchingContext context)
        {
            await ExecuteOnSpecificMessageType(context, message => OnInvocationStarted(message, context.HandlerInstance));
        }

        async Task IContextInterceptor.OnExecuted(DispatchingContext context)
        {
            await ExecuteOnSpecificMessageType(context, message => OnInvocationFinished(message, context.HandlerInstance, context.Result));
        }

        async Task IContextInterceptor.OnException(DispatchingContext context, Exception invocationException)
        {
            await ExecuteOnSpecificMessageType(context, message => OnException(message, context.HandlerInstance, invocationException));
        }

        public abstract Task OnInvocationStarted(TMessage message, object messageHandler);
        public abstract Task OnInvocationFinished(TMessage message, object messageHandler, object invocationResult);
        public abstract Task OnException(TMessage message, object messageHandler, Exception invocationException);

        protected static async Task ExecuteOnSpecificMessageType(DispatchingContext context, Func<TMessage, Task> action)
        {
            await context.With(c => c.MessageInstance).Cast<TMessage>().Do(action);
        }
    }
}