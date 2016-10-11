using System;
using CQRSalad.Dispatching.Context;

namespace CQRSalad.Dispatching.Interceptors
{
    public interface IContextInterceptor
    {
        void OnInvocationStarted(DispatchingContext context);
        void OnInvocationFinished(DispatchingContext context);
        void OnException(DispatchingContext context, Exception invocationException);
    }
}