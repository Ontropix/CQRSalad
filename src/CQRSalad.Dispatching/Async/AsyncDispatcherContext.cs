using System.Reflection;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching.Async
{
    internal sealed class AsyncDispatcherContext : IDispatcherContext
    {
        public object MessageInstance { get; private set; }
        public object HandlerInstance { get; private set; }
        public MethodInfo HandlingMethod { get; private set; }
        public object Result { get; private set; }

        internal AsyncDispatcherContext(object handlerInstance, MethodInfo handlingMethod, object messageInstance)
        {
            HandlingMethod = handlingMethod;
            HandlerInstance = handlerInstance;
            MessageInstance = messageInstance;
        }

        internal async Task<object> InvokeAsync()
        {
            try
            {
                var handlingTask = (Task)HandlingMethod.Invoke(HandlerInstance, new[] { MessageInstance });
                await handlingTask;
                Result = handlingTask.GetType().GetProperty("Result").GetValue(handlingTask);
            }
            catch (TargetInvocationException targetInvocationException)
            {
                ExceptionDispatchInfo.Capture(targetInvocationException.InnerException).Throw();
            }

            return Result;
        }
    }
}