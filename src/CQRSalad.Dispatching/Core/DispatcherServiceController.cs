using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSalad.Dispatching
{
    internal delegate object MessageInvoker(object handler, object message);

    internal class DispatcherHandlersController
    {
        //NOTE: <MessageType, Subscriptions>
        private readonly ConcurrentDictionary<Type, SortedSet<Subscription>> _store = new ConcurrentDictionary<Type, SortedSet<Subscription>>();
        protected virtual Priority DefaultPriorty => Priority.Normal;

        private Func<Type, bool> HandlersTypesResolver { get; }

        //todo: Add validation
        public DispatcherHandlersController(IEnumerable<Type> typesToRegister, Func<Type, bool> handlersResolver = null)
        {
            HandlersTypesResolver = handlersResolver ?? (type => type.IsDefined(typeof(DispatcherHandlerAttribute)));
            Initialize(typesToRegister);
        }

        private void Initialize(IEnumerable<Type> typesToRegister)
        {
            IEnumerable<Type> handlerTypes = typesToRegister
                .Distinct()
                .Where(type => type.IsClass
                               && type.IsPublic
                               && !type.IsAbstract
                               && !type.IsGenericTypeDefinition
                               && !type.ContainsGenericParameters)
                .Where(HandlersTypesResolver);

            foreach (Type handlerType in handlerTypes)
            {
                RegisterHandler(handlerType);
            }
        }

        internal IList<Subscription> GetSubscriptionsFor(Type messageType)
        {
            if (!_store.ContainsKey(messageType))
            {
                throw new HandlerNotFoundException(messageType);
            }
            return _store[messageType].ToList();
        }

        private void RegisterHandler(Type handlerType)
        {
            IEnumerable<MethodInfo> actions = GetHandlerActions(handlerType);
            foreach (MethodInfo action in actions)
            {
                Type messageType = action.GetParameters()[0].ParameterType;
                MessageInvoker invoker = CreateInvoker(messageType, handlerType, action);

                var subscription = new Subscription
                {
                    MessageType = messageType,
                    HandlerType = handlerType,
                    Invoker = invoker,
                    Priority = GetDispatchingPriority(handlerType, action)
                };

                if (!_store.ContainsKey(subscription.MessageType))
                {
                    var comparer = Comparer<Subscription>.Create((d1, d2) => d1.Priority.CompareTo(d2.Priority));
                    _store[subscription.MessageType] = new SortedSet<Subscription>(comparer);
                }

                _store[subscription.MessageType].Add(subscription);
            }
        }

        private IEnumerable<MethodInfo> GetHandlerActions(Type handlerType)
        {
            List<MethodInfo> actions = handlerType
                .GetMethods(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(IsHandlerAction)
                .ToList();
            return actions;
        }

        protected virtual bool IsHandlerAction(MethodInfo method)
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

        protected virtual Priority GetDispatchingPriority(Type handlerType, MethodInfo action)
        {
            Type attributeType = typeof(DispatchingPriorityAttribute);

            if (Attribute.IsDefined(action, attributeType))
            {
                return action.GetCustomAttribute<DispatchingPriorityAttribute>(false).Priority;
            }

            if (Attribute.IsDefined(handlerType, attributeType))
            {
                return handlerType.GetCustomAttribute<DispatchingPriorityAttribute>(false).Priority;
            }

            return DefaultPriorty;
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