using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using CQRSalad.Dispatching.Priority;

namespace CQRSalad.Dispatching
{
    internal class DispatcherSubscriptionsManager
    {
        private readonly IDispatcherTypesProvider _handlersProvider;
        private readonly IDispatcherPriorityProvider _priorityProvider;

        public DispatcherSubscriptionsManager(
            IDispatcherTypesProvider handlersProvider, 
            IDispatcherPriorityProvider priorityProvider
            )
        {
            _handlersProvider = handlersProvider;
            _priorityProvider = priorityProvider;
        }

        public SubscriptionsStore Subscribe()
        {
            var store = new SubscriptionsStore();

            IEnumerable<Type> handlerTypes = GetHandlerTypes();

            foreach (Type handlerType in handlerTypes)
            {
                var actions = GetHandlerActions(handlerType);
                foreach (MethodInfo action in actions)
                {
                    store.Add(new DispatcherSubscription
                    {
                        MessageType = action.GetParameters()[0].ParameterType,
                        HandlerType = handlerType,
                        Action = action,
                        Priority =
                            _priorityProvider.GetActionPriority(action) != Priority.Priority.Unspecified
                                ? action.Priority
                                : handlerDescriptor.Priority //todo move to priority resolver
                    });
                }
            }

            return store;
        }

        public IEnumerable<Type> GetHandlerTypes()
        {
            IEnumerable<Type> types = _handlersProvider.GetTypes();

            var handlerTypes = new HashSet<Type>();
            foreach (Type typeInfo in types)
            {
                if (IsDispatcherHandler(typeInfo))
                {
                    handlerTypes.Add(typeInfo);
                }
            }

            return handlerTypes;
        }

        private bool IsDispatcherHandler(Type typeInfo)
        {
            return typeInfo.IsDefined(typeof(DispatcherHandlerAttribute))
                   && typeInfo.IsClass
                   && typeInfo.IsPublic
                   && !typeInfo.IsAbstract
                   && !typeInfo.IsGenericTypeDefinition
                   && !typeInfo.ContainsGenericParameters;
        }

        public IEnumerable<MethodInfo> GetHandlerActions(Type handlerType)
        {
            List<MethodInfo> actions = handlerType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(IsHandlerAction)
                .ToList();
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