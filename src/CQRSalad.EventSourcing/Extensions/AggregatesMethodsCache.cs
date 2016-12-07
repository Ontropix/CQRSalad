using System;
using System.Collections.Concurrent;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal delegate void CommandHandler(object aggregate, object command);

    internal class CommandHandlerSubscription
    {
        public Type AggregateType { get; set; }
        public Type CommandType { get; set; }
        public CommandHandler Handler { get; set; }
        public bool IsCtor { get; set; }
    }

    internal static class AggregatesMethodsCache
    {
        // Command Type - Aggregate method delegate
        private static readonly ConcurrentDictionary<Type, CommandHandlerSubscription> _cache = new ConcurrentDictionary<Type, CommandHandlerSubscription>();

        internal static CommandHandlerSubscription GetCommandHandler(Type aggregateType, Type commandType)
        {
            if (_cache.ContainsKey(commandType))
            {
                return _cache[commandType];
            }

            MethodInfo action = aggregateType.FindMethodBySinglePameter(commandType);
            if (action == null)
            {
                throw new CommandProcessingException("Aggregate doesn't handle command.");
            }

            var ctor = action.GetCustomAttribute<AggregateCtorAttribute>(false);
            CommandHandler handler = CreateExecutorDelegate(aggregateType, action, commandType);

            var subscription = new CommandHandlerSubscription
            {
                AggregateType = aggregateType,
                CommandType = commandType,
                Handler = handler,
                IsCtor = ctor != null
            };

            _cache.TryAdd(commandType, subscription);
            return subscription;
        }

        private static CommandHandler CreateExecutorDelegate(Type aggregateType, MethodInfo action, Type commandType)
        {
            Type objectType = typeof(object);
            ParameterExpression aggregateParameter = Expression.Parameter(objectType, "aggregate");
            ParameterExpression commandParameter = Expression.Parameter(objectType, "command");

            MethodCallExpression methodCall =
                Expression.Call(
                    Expression.Convert(aggregateParameter, aggregateType),
                    action,
                    Expression.Convert(commandParameter, commandType));

            if (action.ReturnType != typeof(void))
            {
                throw new NotSupportedException("Only void return method allowed."); // todo
            }

            var lambda = Expression.Lambda<CommandHandler>(
                methodCall,
                aggregateParameter,
                commandParameter);

            return lambda.Compile();
        }
    }
}