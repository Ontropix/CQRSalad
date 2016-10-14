using System;
using System.Threading.Tasks;
using CQRSalad.Dispatching.Context;

namespace CQRSalad.Dispatching.Interceptors
{
    public interface IContextInterceptor
    {
        Task OnInvocationStarted(DispatchingContext context);
        Task OnInvocationFinished(DispatchingContext context);
        Task OnException(DispatchingContext context, Exception invocationException);
    }
}