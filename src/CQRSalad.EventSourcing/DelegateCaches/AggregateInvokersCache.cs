using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal class WhenMethod
    {
        public Type AggregateType { get; set; }
        public Type CommandType { get; set; }
        public bool IsCtor { get; set; }
        public Action<object, object> Invoker { get; set; }
    }

    internal class StateOnMethod
    {
        public Type StateType { get; set; }
        public Type EventType { get; set; }
        public Action<object, object> Invoker { get; set; }
    }

    internal static class AggregateInvokersCache
    {
        private static readonly ConcurrentDictionary<Type, WhenMethod> _whenCache =
            new ConcurrentDictionary<Type, WhenMethod>();

        private static readonly ConcurrentDictionary<Type, StateOnMethod> _stateCache =
            new ConcurrentDictionary<Type, StateOnMethod>();
        
        internal static WhenMethod GetWhenMethod(Type aggregateType, Type commandType)
        {
            return _whenCache.GetOrAdd(commandType, key =>
            {
                MethodInfo action = aggregateType.GetMethodWithSingleArgument(commandType);
                if (action == null)
                {
                    return null;
                }

                var ctor = action.GetCustomAttribute<AggregateCtorAttribute>(false);
                return new WhenMethod
                {
                    AggregateType = aggregateType,
                    CommandType = commandType,
                    Invoker = CreateMessageInvoker(aggregateType, action, commandType),
                    IsCtor = ctor != null
                };
            });
        }

        internal static StateOnMethod GetStateOnMethod(Type stateType, Type eventType)
        {
            return _stateCache.GetOrAdd(eventType, key =>
            {
                MethodInfo action = stateType.GetMethodWithSingleArgument(eventType);
                if (action == null)
                {
                    return null;
                }

                return new StateOnMethod
                {
                    StateType = stateType,
                    EventType = eventType,
                    Invoker = CreateMessageInvoker(stateType, action, eventType)
                };
            });
        }

        private static Action<object, object> CreateMessageInvoker(Type targetType, MethodInfo method, Type messageType)
        {
            Type objectType = typeof(object);
            ParameterExpression aggregateParameter = Expression.Parameter(objectType, "target");
            ParameterExpression commandParameter = Expression.Parameter(objectType, "message");

            MethodCallExpression methodCall =
                Expression.Call(
                    Expression.Convert(aggregateParameter, targetType),
                    method,
                    Expression.Convert(commandParameter, messageType));

            var lambda = Expression.Lambda<Action<object, object>>(
                methodCall,
                aggregateParameter,
                commandParameter);

            return lambda.Compile();
        }
    }
}