using System;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
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

            if (Aggregate.Changes.Count < 1)
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