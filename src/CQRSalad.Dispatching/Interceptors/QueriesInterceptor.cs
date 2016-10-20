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
        async Task IContextInterceptor.OnExecuting(DispatchingContext context)
        {
            await Execute(context, message => OnExecuting(context));
        }

        async Task IContextInterceptor.OnExecuted(DispatchingContext context)
        {
            await Execute(context, message => OnExecuted(context));
        }

        async Task IContextInterceptor.OnException(DispatchingContext context, Exception invocationException)
        {
            await Execute(context, message => OnException(context));
        }

        public abstract Task OnExecuting(DispatchingContext context);
        public abstract Task OnExecuted(DispatchingContext context);
        public abstract Task OnException(DispatchingContext context);

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