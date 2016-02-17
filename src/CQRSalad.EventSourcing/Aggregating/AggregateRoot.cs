using System;
using System.Collections.Generic;
using System.Reflection;
using CQRSalad.EventStore.Core;

namespace CQRSalad.EventSourcing
{
    public abstract class AggregateRoot
    {
        internal string Id { get; set; }
        internal int Version { get; set; }
        internal List<IEvent> Changes { get; }
        internal virtual bool HasState => false;

        protected AggregateRoot()
        {
            Changes = new List<IEvent>();
        }

        internal virtual void Reel(List<IEvent> events)
        {
            Argument.ElementsNotNull(events);
            Version += events.Count;
        }

        protected virtual void ProduceEvent(IEvent evnt)
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

        internal sealed override void Reel(List<IEvent> events)
        {
            base.Reel(events);
            foreach (var @event in events)
            {
                ApplyEventOnState(@event);
            }
        }

        protected sealed override void ProduceEvent(IEvent evnt)
        {
            Argument.IsNotNull(evnt, nameof(evnt));

            ApplyEventOnState(evnt);
            base.ProduceEvent(evnt);
        }

        protected void ProduceError(string errorMessage)
        {
            throw new InvalidAggregateStateException(errorMessage);
        }

        protected void ProduceError(string errorMessage, Func<TState, bool> condition)
        {
            if (condition.Invoke(State))
            {
                ProduceError(errorMessage);
            }
        }

        private void ApplyEventOnState(IEvent evnt)
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