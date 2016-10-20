using System;
using System.Threading.Tasks;
using CQRSalad.Dispatching.Context;

namespace CQRSalad.Dispatching.Interceptors
{
    public interface IContextInterceptor
    {
        Task OnExecuting(DispatchingContext context);
        Task OnExecuted(DispatchingContext context);
        Task OnException(DispatchingContext context, Exception invocationException);
    }
}