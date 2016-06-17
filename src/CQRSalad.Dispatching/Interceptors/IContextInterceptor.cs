using System;
using CQRSalad.Dispatching.NEW.Context;

namespace CQRSalad.Dispatching
{
    public interface IContextInterceptor
    {
        void OnInvocationStarted(DispatchingContext context);
        void OnInvocationFinished(DispatchingContext context);
        void OnException(DispatchingContext context, Exception invocationException);
    }
}