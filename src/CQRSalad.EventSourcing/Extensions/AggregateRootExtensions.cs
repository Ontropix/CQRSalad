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

            if (aggregate.Status == RootStatus.New && !subscription.IsConstructor)
            {
                throw new InvalidOperationException("Attempting to apply a command to non existed aggregate.");
            }
            
            if (aggregate.Status != RootStatus.New && subscription.IsConstructor)
            {
                throw new InvalidOperationException("Attempting to create existed aggregate.");
            }

            if (aggregate.Status == RootStatus.Archived)
            {
                throw new InvalidOperationException("Aggregate is archived.");
            }

            subscription.Invoker(aggregate, command);

            if (aggregate.Changes.Count < 1)
            {
                throw new InvalidOperationException($"Command '{command.GetType().AssemblyQualifiedName}' produced no events");
            }

            aggregate.Status = RootStatus.Alive;
            aggregate.Reel(aggregate.Changes);

            if (subscription.IsDestructor)
            {
                aggregate.Status = RootStatus.Archived;
            }
        }
        
        internal static void SetStatus(this IAggregateRoot root, int streamStartIndex, bool isEnded)
        {
            if (root.Version >= streamStartIndex && !isEnded)
            {
                root.Status = RootStatus.Alive;
                return;
            }

            if (root.Version >= streamStartIndex && isEnded)
            {
                root.Status = RootStatus.Archived;
                return;
            }

            if (root.Version < streamStartIndex && !isEnded)
            {
                root.Status = RootStatus.New;
                return;
            }

            throw new InvalidOperationException("Status is unknown!");
        }

        internal static void Reel(this IAggregateRoot root, IEnumerable<object> events)
        {
            foreach (var @event in events)
            {
                ApplyOnState(root, @event);
            }
        }

        private static void ApplyOnState(this IAggregateRoot root, object evnt)
        {
            StateOnMethod subscription = AggregateInvokersCache.GetStateOnMethod(root.State.GetType(), evnt.GetType());
            subscription?.Invoker(root.State, evnt);
        }
    }
}