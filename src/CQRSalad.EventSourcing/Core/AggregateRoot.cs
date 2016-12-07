using System;
using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    public class AggregateActivator
    {
        public static TAggregate CreateInstance<TAggregate>(string aggregateId) where TAggregate : AggregateRoot, new()
        {
            Argument.StringNotEmpty(aggregateId, nameof(aggregateId));
            return new TAggregate { Id = aggregateId};
        }
    }

    public abstract class AggregateRoot
    {
        public string Id { get; internal set; }
        public int Version { get; internal set; }
        internal List<IEvent> Changes { get; } //todo change type for safety collection
        internal virtual bool HasState => false;

        protected AggregateRoot()
        {
            Changes = new List<IEvent>();
        }

        public virtual void Reel(List<IEvent> events)
        {
            Argument.ElementsNotNull(events);
            Version += events.Count;
        }

        protected virtual void ProduceEvent<TEvent>(TEvent evnt) where TEvent : class, IEvent
        {
            Argument.IsNotNull(evnt, nameof(evnt));

            Changes.Add(evnt);
            Version++;
        }
    }

    public abstract class AggregateRoot<TState> : AggregateRoot where TState : class, new()
    {
        protected internal TState State { get; internal set; }
        internal sealed override bool HasState => true;

        protected AggregateRoot()
        {
            State = new TState();
        }

        public sealed override void Reel(List<IEvent> events)
        {
            base.Reel(events);
            foreach (var @event in events)
            {
                ApplyOnState(@event);
            }
        }

        protected sealed override void ProduceEvent<TEvent>(TEvent evnt)
        {
            Argument.IsNotNull(evnt, nameof(evnt));

            ApplyOnState(evnt);
            base.ProduceEvent(evnt);
        }

        protected void ProduceError(string errorMessage)
        {
            throw new InvalidAggregateStateException(errorMessage);
        }
        
        private void ApplyOnState(object evnt)
        {
            MethodInfo action = FindStateMethod(evnt);
            action?.Invoke(State, new object[] { evnt });
        }

        private MethodInfo FindStateMethod(object evnt)
        {
            return typeof(TState).FindMethodBySinglePameter(evnt.GetType());
        }
    }
}