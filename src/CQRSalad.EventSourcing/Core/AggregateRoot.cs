using System;
using System.Collections.Generic;

namespace CQRSalad.EventSourcing
{
    public abstract class AggregateRoot<TState> : IAggregateRoot where TState : class, new()
    {
        string IAggregateRoot.Id { get; set; }
        int IAggregateRoot.Version { get; set; }
        bool IAggregateRoot.IsFinalized { get; set; }
        object IAggregateRoot.State { get { return State; } set { State = (TState) value; } }
        List<IEvent> IAggregateRoot.Changes => _changes;
        
        private readonly List<IEvent> _changes = new List<IEvent>();
        protected TState State { get; private set; } = new TState();

        protected void ProduceEvent<TEvent>(TEvent evnt) where TEvent : class, IEvent
        {
            Argument.IsNotNull(evnt, nameof(evnt));
            _changes.Add(evnt);
        }

        protected void ProduceError(string errorMessage)
        {
            throw new InvalidAggregateStateException(errorMessage);
        }
    }
}