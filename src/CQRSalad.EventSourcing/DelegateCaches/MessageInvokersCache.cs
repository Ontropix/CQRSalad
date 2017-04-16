using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal class MessageInvokersCache<TDelegate, TSubscription> 
        where TDelegate : class
        where TSubscription : class
    {
        private readonly Func<TDelegate, TSubscription> _newItemFunc;
        private readonly ConcurrentDictionary<Type, TSubscription> _cache = new ConcurrentDictionary<Type, TSubscription>();

        internal MessageInvokersCache(Func<TDelegate, TSubscription> newItemFunc)
        {
            Argument.IsNotNull(newItemFunc, nameof(newItemFunc));
            _newItemFunc = newItemFunc;
        }

        internal TSubscription GetMessageInvoker(Type target, Type message, Func<TDelegate, TSubscription> newItemFunc)
        {
            if (_cache.ContainsKey(message))
            {
                return _cache[message];
            }

            MethodInfo action = target.GetMethodWithSingleArgument(message);
            if (action == null)
            {
                return default(TSubscription);
            }

            var invoker = CreateMessageInvoker(target, action, message);
            var subscription = newItemFunc(invoker);

            _cache.TryAdd(message, subscription);
            return subscription;
        }

        internal TDelegate CreateMessageInvoker(Type targetType, MethodInfo method, Type messageType)
        {
            Type objectType = typeof(object);
            ParameterExpression aggregateParameter = Expression.Parameter(objectType, "target");
            ParameterExpression commandParameter = Expression.Parameter(objectType, "message");

            MethodCallExpression methodCall =
                Expression.Call(
                    Expression.Convert(aggregateParameter, targetType),
                    method,
                    Expression.Convert(commandParameter, messageType));

            var lambda = Expression.Lambda<TDelegate>(
                methodCall,
                aggregateParameter,
                commandParameter);

            return lambda.Compile();
        }
    }
}