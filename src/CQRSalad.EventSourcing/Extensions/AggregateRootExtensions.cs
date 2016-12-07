using System;
using CQRSalad.Domain;

namespace CQRSalad.EventSourcing
{
    public static class AggregateRootExtensions
    {
        internal static void Perform<TCommand>(this AggregateRoot aggregate, TCommand command)
            where TCommand : class, ICommand
        {
            Type aggregateType = aggregate.GetType();
            Type commandType = command.GetType();

            var subscription = AggregatesMethodsCache.GetCommandHandler(aggregateType, commandType);
            
            if (!subscription.IsCtor && aggregate.Version == 0)
            {
                throw new InvalidOperationException("Attempting to create an aggregate using non-constructor command.");
            }

            if (subscription.IsCtor && aggregate.Version > 0)
            {
                throw new InvalidOperationException("Attempting to create existed aggregate.");
            }

            subscription.Handler(aggregate, command);

            if (aggregate.Changes.Count < 1)
            {
                throw new CommandProducedNoEventsException(command);
            }
        }
    }
}