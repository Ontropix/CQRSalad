using System;

namespace CQRSalad.Dispatching
{
    public interface IContextInterceptor
    {
        void OnInvocationStarted(IDispatcherContext context);
        void OnInvocationFinished(IDispatcherContext context);
        void OnException(IDispatcherContext context, Exception invocationException);
    }
}