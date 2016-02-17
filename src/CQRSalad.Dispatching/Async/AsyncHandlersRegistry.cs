using System.Reflection;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching.Async
{
    internal sealed class AsyncHandlersRegistry : HandlersRegistry
    {
        internal AsyncHandlersRegistry(bool isHandlingPriorityEnabled)
            : base(isHandlingPriorityEnabled)
        {
        }

        protected override bool MethodsFilter(MethodInfo method)
        {
            return base.MethodsFilter(method) && IsAsync(method);
        }

        private static bool IsAsync(MethodInfo method)
        {
            var asyncAttribute = method.GetCustomAttribute<AsyncStateMachineAttribute>();
            return asyncAttribute != null && typeof(Task).IsAssignableFrom(method.ReturnType);
        }
    }
}