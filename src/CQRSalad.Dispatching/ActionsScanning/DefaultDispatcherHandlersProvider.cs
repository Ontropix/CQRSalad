using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CQRSalad.Dispatching.ActionsScanning
{
    public class DefaultDispatcherHandlerActionsProvider : IDispatcherHandlerActionsProvider
    {
        public IEnumerable<MethodInfo> GetHandlerActions(TypeInfo handlerType)
        {
            List<MethodInfo> actions = handlerType.GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly).Where(IsHandlerAction).ToList();
            return actions;
        }

        private bool IsHandlerAction(MethodInfo method)
        {
            bool isDefinitionMatch = method.IsPublic &&
                                        method.GetParameters().Length == 1 &&
                                        !method.IsAbstract &&
                                        !method.ContainsGenericParameters &&
                                        !method.IsConstructor &&
                                        !method.IsGenericMethod &&
                                        !method.IsStatic;

            return isDefinitionMatch;
        }
    }
}