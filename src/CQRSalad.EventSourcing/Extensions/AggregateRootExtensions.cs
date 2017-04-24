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

            if (aggregate.Version == 0 && !subscription.IsConstructor)
            {
                throw new InvalidOperationException("Attempting to apply a command to non existed aggregate.");
            }
            
            if (aggregate.Version > 0 && subscription.IsConstructor)
            {
                throw new InvalidOperationException("Attempting to create existed aggregate.");
            }

            if (aggregate.IsFinalized)
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
                aggregate.IsFinalized = true;
            }

            aggregate.Reel(aggregate.Changes);
        }

        internal static void Restore(this IAggregateRoot aggregate, EventStream stream)
        {
            if (stream == null)
            {
                return;
            }

            //if (aggregate.Id != stream.Meta.AggregateId)
            //{
            //    throw new InvalidOperationException("Invalid aggregateId in snapshopt.");
            //}

            //if (aggregate.GetType() != stream.Meta.AggregateType)
            //{
            //    throw new InvalidOperationException("Trying to restore aggregate with wrong snapshot type.");
            //}

            aggregate.Version = stream.Version;
            aggregate.IsFinalized = stream.IsEnded;
            aggregate.Reel(stream.Events);
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