using System;
using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    public abstract class AggregateRoot
    {
        public string Id { get; set; }
        public int Version { get; set; }
        public List<IEvent> Changes { get; }
        public virtual bool HasState => false; //todo public?

        protected AggregateRoot()
        {
            Changes = new List<IEvent>();
        }

        internal virtual void Reel(List<IEvent> events)
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
        public sealed override bool HasState => true;

        protected AggregateRoot()
        {
            State = new TState();
        }

        internal sealed override void Reel(List<IEvent> events)
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