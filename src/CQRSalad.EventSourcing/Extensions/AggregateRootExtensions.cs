using System;
using System.Collections.Generic;

namespace CQRSalad.EventSourcing
{
    internal static class AggregateRootExtensions
    {
        internal static void Perform<TCommand>(this IAggregateRoot aggregate, TCommand command)
        {
            Type aggregateType = aggregate.GetType();
            Type commandType = command.GetType();

            var subscription = AggregateInvokersCache.GetWhenMethod(aggregateType, commandType);
            if (subscription == null)
            {
                throw new InvalidOperationException("Aggregate can't handle command.");
            }

            if (!subscription.IsCtor && aggregate.Version == 0)
            {
                throw new InvalidOperationException("Attempting to create an aggregate using non-constructor command.");
            }

            if (subscription.IsCtor && aggregate.Version > 0)
            {
                throw new InvalidOperationException("Attempting to create existed aggregate.");
            }

            subscription.Invoker(aggregate, command);

            if (aggregate.Changes.Count < 1)
            {
                throw new InvalidOperationException($"Command '{command.GetType().AssemblyQualifiedName}' produced no events");
            }

            aggregate.Reel(aggregate.Changes);
        }

        //todo State Null checking
        internal static void Reel(this IAggregateRoot root, List<IEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyOnState(root, @event);
            }

            root.Version += events.Count;//todo Version depends on Events Count??
        }

        internal static void ApplyOnState(this IAggregateRoot root, IEvent evnt)
        {
            StateOnMethod subscription = AggregateInvokersCache.GetStateOnMethod(root.State.GetType(), evnt.GetType());
            subscription?.Invoker(root.State, evnt);
        }
    }
}