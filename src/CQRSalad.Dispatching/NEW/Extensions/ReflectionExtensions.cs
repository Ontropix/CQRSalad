using System;
using System.Reflection;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    internal static class ReflectionExtensions
    {
        internal static bool IsAsync(this MethodInfo method)
        {
            return typeof(Task).IsAssignableFrom(method.ReturnType);
        }

        internal static Type GetAsyncMethodTaskReturnType(this MethodInfo method)
        {
            Type type = method.ReturnType;
            if (IsAsync(method) && type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Task<>))
            {
                return type.GetGenericArguments()[0];
            }

            return null;
        }
    }
}