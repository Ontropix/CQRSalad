using System;
using System.Collections.Generic;

namespace CQRSalad.EventSourcing
{
    internal static class AggregateRootExtensions
    {
        internal static void Perform<TCommand>(this IAggregateRoot aggregate, TCommand command)
        {
            var subscription = AggregateInvokersCache.GetWhenMethod(aggregate.GetType(), command.GetType());
            if (subscription == null)
            {
                throw new InvalidOperationException("Aggregate can't handle command.");
            }

            if (aggregate.Status == AggregateStatus.New && !subscription.IsConstructor)
            {
                throw new InvalidOperationException("Attempting to apply a command to non existed aggregate.");
            }
            
            if (aggregate.Status != AggregateStatus.New && subscription.IsConstructor)
            {
                throw new InvalidOperationException("Attempting to create existed aggregate.");
            }

            if (aggregate.Status == AggregateStatus.Finalized)
            {
                throw new InvalidOperationException("Aggregate is finalized.");
            }

            subscription.Invoker(aggregate, command);

            if (aggregate.Changes.Count < 1)
            {
                throw new InvalidOperationException($"Command '{command.GetType().AssemblyQualifiedName}' produced no events");
            }

            if (subscription.IsDestructor)
            {
                aggregate.Status = AggregateStatus.Finalized;
            }

            aggregate.Reel(aggregate.Changes);
        }

        internal static void Restore(this IAggregateRoot root, EventStream stream)
        {
            if (stream == null)
            {
                // if no stream = empty aggregate
                root.Status = AggregateStatus.New;
                root.Version = -1;
                return;
            }

            root.Id = stream.StreamId;
            root.Status = stream.Metadata.AggregateStatus;
            root.Version = stream.Version;

            root.Reel(stream.Events);
        }

        //todo State Null checking
        internal static void Reel(this IAggregateRoot root, IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                ApplyOnState(root, @event);
            }
        }

        internal static void ApplyOnState(this IAggregateRoot root, IEvent evnt)
        {
            StateOnMethod subscription = AggregateInvokersCache.GetStateOnMethod(root.State.GetType(), evnt.GetType());
            subscription?.Invoker(root.State, evnt);
        }
    }
}