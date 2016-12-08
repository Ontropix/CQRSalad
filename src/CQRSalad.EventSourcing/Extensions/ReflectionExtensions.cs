using System;
using System.Linq;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal static class ReflectionExtensions
    {
        internal static MethodInfo FindMethodBySinglePameter(this IReflect methodOwner, Type parameterType)
        {
            //we can take First element, because we filter methods with definition: "public void ACTION_NAME (MESSAGE_TYPE message)"
            MethodInfo action = methodOwner.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly)
                                           .FirstOrDefault(_method => MethodsFilter(_method, parameterType));
            return action;
        }

        private static bool MethodsFilter(MethodInfo method, Type parameterType)
        {
            ParameterInfo[] parameters = method.GetParameters();

            return method.IsPublic &&
                   method.ReturnType == typeof(void) &&

                   parameters.Length == 1 &&
                   parameters[0].ParameterType == parameterType &&

                   !method.IsAbstract &&
                   !method.ContainsGenericParameters &&
                   !method.IsConstructor &&
                   !method.IsGenericMethod &&
                   !method.IsStatic;
        }
    }
}
