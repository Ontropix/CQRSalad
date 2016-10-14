using System;
using System.Linq;
using System.Threading.Tasks;
using CQRSalad.Dispatching.Context;
using CQRSalad.Domain;
using CQRSalad.Infrastructure;

namespace CQRSalad.Dispatching.Interceptors
{
    public abstract class QueriesInterceptor : IContextInterceptor
    {
        async Task IContextInterceptor.OnInvocationStarted(DispatchingContext context)
        {
            await Execute(context, message => OnInvocationStarted(message, context.HandlerInstance));
        }

        async Task IContextInterceptor.OnInvocationFinished(DispatchingContext context)
        {
            await Execute(context, message => OnInvocationFinished(message, context.HandlerInstance, context.Result));
        }

        async Task IContextInterceptor.OnException(DispatchingContext context, Exception invocationException)
        {
            await Execute(context, message => OnException(message, context.HandlerInstance, invocationException));
        }

        public abstract Task OnInvocationStarted(object message, object messageHandler);
        public abstract Task OnInvocationFinished(object message, object messageHandler, object invocationResult);
        public abstract Task OnException(object message, object messageHandler, Exception invocationException);

        protected static async Task Execute(DispatchingContext context, Func<object, Task> action)
        {
            await context.With(c => c.MessageInstance)
                .If(
                    message =>
                        message.GetType()
                            .GetInterfaces()
                            .Any(x => x.IsGenericType && x.GetGenericTypeDefinition() == typeof(IQueryFor<>)))
                .Do(action);
        }
    }
}