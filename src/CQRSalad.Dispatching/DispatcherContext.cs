using System.Reflection;

namespace CQRSalad.Dispatching
{
    internal sealed class DispatcherContext : IDispatcherContext
    {
        public object MessageInstance { get; private set; }
        public object HandlerInstance { get; private set; }
        public MethodInfo HandlingMethod { get; private set; }
        public object Result { get; private set; }

        internal DispatcherContext(object handlerInstance, MethodInfo handlingMethod, object messageInstance)
        {
            HandlingMethod = handlingMethod;
            HandlerInstance = handlerInstance;
            MessageInstance = messageInstance;
        }

        internal object Invoke()
        {
            Result = HandlingMethod.Invoke(HandlerInstance, new[] { MessageInstance });
            return Result;
        }
    }
}