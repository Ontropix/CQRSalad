using System;
using System.Collections.Generic;

namespace CQRSalad.EventSourcing
{
    public abstract class AggregateRoot<TState> : IAggregateRoot where TState : class, new()
    {
        string IAggregateRoot.Id  { get { return Id; } set { Id = value; } }
        int IAggregateRoot.Version { get { return Version; } set { Version = value; } }
        object IAggregateRoot.State { get { return State; } set { State = (TState) value; } }
        List<IEvent> IAggregateRoot.Changes => _changes;

        //todo State Null checking
        void IAggregateRoot.Reel(List<IEvent> events)
        {
            Version += events.Count;
            foreach (var @event in events)
            {
                ApplyOnState(@event);
            }
        }

        internal string Id { get; set; }
        internal int Version { get; private set; }
        private readonly List<IEvent> _changes = new List<IEvent>();
        protected internal TState State { get; private set; } = new TState();

        protected void ProduceEvent<TEvent>(TEvent evnt) where TEvent : class, IEvent
        {
            Argument.IsNotNull(evnt, nameof(evnt));

            ApplyOnState(evnt);

            _changes.Add(evnt);
            Version++;
        }

        protected void ProduceError(string errorMessage)
        {
            throw new InvalidAggregateStateException(errorMessage);
        }
        
        private void ApplyOnState(object evnt)
        {
            EventApplierSubscription subscription = AggregatesStateMethodsCache.GetEventApplier(typeof(TState), evnt.GetType());
            subscription.Handler(State, evnt);
        }
    }
}