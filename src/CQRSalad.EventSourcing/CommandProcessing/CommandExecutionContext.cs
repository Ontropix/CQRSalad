using System;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal delegate void AggregateExecutor(object aggregate, object command);

    internal class AggregateExecutorsManager
    {
        internal AggregateExecutor GetExecutor(Type aggregateType, MethodInfo action, Type commandType)
        {
            //todo cache

            AggregateExecutor executor = CreateExecutorDelegate(aggregateType, action, commandType);
            return executor;
        }

        private static AggregateExecutor CreateExecutorDelegate(Type aggregateType, MethodInfo action, Type commandType)
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

            var lambda = Expression.Lambda<AggregateExecutor>(
                methodCall,
                aggregateParameter,
                commandParameter);

            return lambda.Compile();
        }
    }

    internal class CommandExecutionContext<TCommand> where TCommand : class
    {
        protected AggregateRoot Aggregate { get; }
        protected TCommand Command { get; }

        public CommandExecutionContext(AggregateRoot aggregate, TCommand command)
        {
            Argument.IsNotNull(aggregate, nameof(aggregate));
            Argument.IsNotNull(command, nameof(command));

            Aggregate = aggregate;
            Command = command;
        }

        public virtual void Perform()
        {
            MethodInfo action = FindMethodFor(Command);
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

            action.Invoke(Aggregate, new object[] { Command });

            if (Aggregate.UncommittedEvents.Count < 1)
            {
                throw new CommandProducedNoEventsException(Command);
            }
        }

        private MethodInfo FindMethodFor(object command)
        {
            return Aggregate.GetType().FindMethodBySinglePameter(command.GetType());
        }
    }
}