using System;
using CQRSalad.Dispatching.NEW.Context;
using CQRSalad.Infrastructure;

namespace CQRSalad.Dispatching.Interceptors
{
    public abstract class SpecificMessageInterceptor<TMessage> : IContextInterceptor where TMessage : class
    {
        void IContextInterceptor.OnInvocationStarted(DispatchingContext context)
        {
            ExecuteOnSpecificMessageType(context, message => OnInvocationStarted(message, context.HandlerInstance));
        }

        void IContextInterceptor.OnInvocationFinished(DispatchingContext context)
        {
            ExecuteOnSpecificMessageType(context, message => OnInvocationFinished(message, context.HandlerInstance, context.Result));
        }

        void IContextInterceptor.OnException(DispatchingContext context, Exception invocationException)
        {
            ExecuteOnSpecificMessageType(context, message => OnException(message, context.HandlerInstance, invocationException));
        }

        public abstract void OnInvocationStarted(TMessage message, object messageHandler);
        public abstract void OnInvocationFinished(TMessage message, object messageHandler, object invocationResult);
        public abstract void OnException(TMessage message, object messageHandler, Exception invocationException);

        protected static void ExecuteOnSpecificMessageType(DispatchingContext context, Action<TMessage> action)
        {
            context.With(c => c.MessageInstance)
                   .Cast<TMessage>()
                   .Do(action.Invoke);
        }
    }
}