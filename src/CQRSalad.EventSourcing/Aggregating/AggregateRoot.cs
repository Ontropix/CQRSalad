using System;
using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    public abstract class AggregateRoot
    {
        internal string Id { get; set; }
        internal int Version { get; set; }
        internal List<object> Changes { get; }
        internal virtual bool HasState => false;

        protected AggregateRoot()
        {
            Changes = new List<object>();
        }

        internal virtual void Reel(List<object> events)
        {
            Argument.ElementsNotNull(events);
            Version += events.Count;
        }

        protected virtual void ProduceEvent<TEvent>(TEvent evnt) where TEvent : class
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

        internal sealed override void Reel(List<object> events)
        {
            base.Reel(events);
            foreach (var @event in events)
            {
                ApplyEventOnState(@event);
            }
        }

        protected sealed override void ProduceEvent<TEvent>(TEvent evnt)
        {
            Argument.IsNotNull(evnt, nameof(evnt));

            ApplyEventOnState(evnt);
            base.ProduceEvent(evnt);
        }

        protected void ProduceError(string errorMessage)
        {
            throw new InvalidAggregateStateException(errorMessage);
        }
        
        private void ApplyEventOnState(object evnt)
        {
            MethodInfo action = FindStateMethod(evnt);
            action?.Invoke(State, new [] { evnt });
        }

        private MethodInfo FindStateMethod(object evnt)
        {
            return typeof(TState).FindMethodBySinglePameter(evnt.GetType());
        }
    }
}