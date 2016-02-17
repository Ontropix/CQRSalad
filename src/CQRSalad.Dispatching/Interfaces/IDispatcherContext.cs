using System.Reflection;

namespace CQRSalad.Dispatching
{
    public interface IDispatcherContext
    {
        object MessageInstance { get; }
        object HandlerInstance { get; }
        MethodInfo HandlingMethod { get; }
        object Result { get; }
    }
}