using System;
using CQRSalad.Infrastructure;

namespace CQRSalad.Dispatching.Interceptors
{
    public abstract class SpecificMessageInterceptor<TMessage> : IContextInterceptor where TMessage : class
    {
        void IContextInterceptor.OnInvocationStarted(IDispatcherContext context)
        {
            ExecuteOnSpecificMessageType(context, message => OnInvocationStarted(message, context.HandlerInstance));
        }

        void IContextInterceptor.OnInvocationFinished(IDispatcherContext context)
        {
            ExecuteOnSpecificMessageType(context, message => OnInvocationFinished(message, context.HandlerInstance, context.Result));
        }

        void IContextInterceptor.OnException(IDispatcherContext context, Exception invocationException)
        {
            ExecuteOnSpecificMessageType(context, message => OnException(message, context.HandlerInstance, invocationException));
        }

        public abstract void OnInvocationStarted(TMessage message, object messageHandler);
        public abstract void OnInvocationFinished(TMessage message, object messageHandler, object invocationResult);
        public abstract void OnException(TMessage message, object messageHandler, Exception invocationException);

        protected static void ExecuteOnSpecificMessageType(IDispatcherContext context, Action<TMessage> action)
        {
            context.With(c => c.MessageInstance)
                   .Cast<TMessage>()
                   .Do(action.Invoke);
        }
    }
}