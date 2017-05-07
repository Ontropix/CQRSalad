using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CQRSalad.EventSourcing
{
    [DebuggerDisplay("Type={GetType().Name}, Version={((IAggregateRoot)this).Version}")]
    public abstract class AggregateRoot<TState> : IAggregateRoot where TState : class, new()
    {
        string IAggregateRoot.Id { get; set; }
        int IAggregateRoot.Version { get; set; }
        AggregateStatus IAggregateRoot.Status { get; set; }
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