using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal static class ReflectionExtensions
    {
        internal static MethodInfo FindMethodBySinglePameter(this IReflect target, Type parameterType)
        {
            //we can take First element, because we filter methods with definition: "public void ACTION_NAME (MESSAGE_TYPE message)"
            return GetMethodsWithSingleParameter(target).FirstOrDefault(method => method.GetParameters()[0].ParameterType == parameterType);
        }

        internal static IEnumerable<MethodInfo> GetMethodsWithSingleParameter(this IReflect target)
        {
            IEnumerable<MethodInfo> actions = target.GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly).Where(MethodsFilter);
            return actions;
        }

        private static bool MethodsFilter(MethodInfo method)
        {
            return method.IsPublic &&
                   method.ReturnType == typeof (void) &&

                   method.GetParameters().Length == 1 &&
                   !method.IsAbstract &&
                   !method.ContainsGenericParameters &&
                   !method.IsConstructor &&
                   !method.IsGenericMethod &&
                   !method.IsStatic;
        }
    }
}
