using System;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    public interface IContextInterceptor
    {
        Task OnExecuting(DispatchingContext context);
        Task OnExecuted(DispatchingContext context);
        Task OnException(DispatchingContext context, Exception invocationException);
    }
}