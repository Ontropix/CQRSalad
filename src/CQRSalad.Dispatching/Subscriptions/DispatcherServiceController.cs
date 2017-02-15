using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSalad.Dispatching
{
    internal delegate object MessageInvoker(object handler, object message);

    public class DispatcherServiceController
    {
        private const Priority DefaultPriorty = Priority.Normal;

        internal SubscriptionsStore Register(IEnumerable<Type> typesToRegister)
        {
            var store = new SubscriptionsStore();

            IEnumerable<Type> handlerTypes = typesToRegister.Distinct().Where(IsDispatcherHandler);
            foreach (Type handlerType in handlerTypes)
            {
                var actions = GetHandlerActions(handlerType);
                foreach (MethodInfo action in actions)
                {
                    Type messageType = action.GetParameters()[0].ParameterType;
                    MessageInvoker invoker = CreateInvoker(messageType, handlerType, action);

                    var subscription = new DispatcherSubscription
                    {
                        MessageType = messageType,
                        HandlerType = handlerType,
                        Invoker = invoker,
                        Priority = GetPriority(handlerType, action)
                    };

                    store.Add(subscription);
                }
            }

            return store;
        }

        private bool IsDispatcherHandler(Type type)
        {
            return type.IsDefined(typeof(DispatcherHandlerAttribute))
                   && type.IsClass
                   && type.IsPublic
                   && !type.IsAbstract
                   && !type.IsGenericTypeDefinition
                   && !type.ContainsGenericParameters;
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
        
        public Priority GetPriority(Type handlerType, MethodInfo action)
        {
            Type attributeType = typeof(DispatchingPriorityAttribute);

            if (Attribute.IsDefined(action, attributeType))
            {
                return GetPriorityFromAttribute(action);
            }

            if (Attribute.IsDefined(handlerType, attributeType))
            {
                return GetPriorityFromAttribute(handlerType);
            }

            return DefaultPriorty;
        }

        private Priority GetPriorityFromAttribute(MemberInfo member)
        {
            var priorityAttribute = member.GetCustomAttribute<DispatchingPriorityAttribute>(false);
            return priorityAttribute.Priority;
        }

        private static MessageInvoker CreateInvoker(Type messageType, Type handlerType, MethodInfo action)
        {
            Type objectType = typeof(object);
            ParameterExpression handlerParameter = Expression.Parameter(objectType, "handler");
            ParameterExpression messageParameter = Expression.Parameter(objectType, "message");

            MethodCallExpression methodCall =
                Expression.Call(
                    Expression.Convert(handlerParameter, handlerType),
                    action,
                    Expression.Convert(messageParameter, messageType));

            if (action.ReturnType == typeof(void))
            {
                var lambda = Expression.Lambda<Action<object, object>>(
                    methodCall,
                    handlerParameter,
                    messageParameter);

                Action<object, object> voidExecutor = lambda.Compile();
                return (handler, message) =>
                {
                    voidExecutor(handler, message);
                    return null;
                };
            }
            else
            {
                var lambda = Expression.Lambda<MessageInvoker>(
                    Expression.Convert(methodCall, typeof(object)),
                    handlerParameter,
                    messageParameter);

                return lambda.Compile();
            }
        }
    }
}