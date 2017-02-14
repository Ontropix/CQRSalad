using System;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    internal interface IInterceptorsManager
    {
        void RegisterInterceptor(Type interceptorType);
    }

    public interface IContextInterceptor
    {
        Task OnExecuting(DispatchingContext context);
        Task OnExecuted(DispatchingContext context);
        Task OnException(DispatchingContext context, Exception invocationException);
    }
}