using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal delegate void CommandExecutor(DomainContext context);

    internal class CommandExecutorsManager
    {
        internal CommandExecutor GetExecutor(Type aggregateType, Type commandType)
        {
            //todo cache

            MethodInfo action = aggregateType.FindMethodBySinglePameter(commandType);
            if (action == null)
            {
                throw new CommandProcessingException("Aggregate doesn't handle command.");
            }

            var ctor = action.GetCustomAttribute<AggregateCtorAttribute>(false);

            if (ctor == null && Aggregate.Version == 0)
            {
                throw new InvalidOperationException("Attempting to create an aggregate using non-constructor command.");
            }

            if (ctor != null && Aggregate.Version > 0)
            {
                throw new InvalidOperationException("Attempting to create existed aggregate.");
            }

            action.Invoke(Aggregate, new object[] {Command});

            if (Aggregate.Changes.Count < 1)
            {
                throw new CommandProducedNoEventsException(Command);
            }

            CommandExecutor executor = CreateExecutorDelegate(aggregateType, action, commandType);
            return executor;
        }

        private static CommandExecutor CreateExecutorDelegate(Type aggregateType, MethodInfo action, Type commandType)
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

            var lambda = Expression.Lambda<CommandExecutor>(
                methodCall,
                aggregateParameter,
                commandParameter);

            return lambda.Compile();
        }
    }
}