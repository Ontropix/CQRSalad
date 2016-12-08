using System;
using System.Collections.Generic;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal class BB
    {
    }

    internal interface IAggregateRoot
    {
        string Id { get; set; }
        int Version { get; set; }
        List<IEvent> Changes { get; } 
        void Reel(List<IEvent> events);
        object State { get; set; }
    }

    public abstract class AggregateRoot<TState> : IAggregateRoot where TState : class, new()
    {
        string IAggregateRoot.Id
        {
            get { return this.Id; }
            set { this.Id = value; }
        }

        object IAggregateRoot.State
        {
            get { return State; }
            set { State = (TState) value; }
        }

        protected internal TState State { get; private set; } = new TState();

        internal string Id { get; set; }



        public int Version { get; set; }

        public List<IEvent> Changes { get; } = new List<IEvent>();

        public bool HasState => true;


        public void Reel(List<IEvent> events)
        {
            Version += events.Count;
            foreach (var @event in events)
            {
                ApplyOnState(@event);
            }
        }
        
        protected void ProduceEvent<TEvent>(TEvent evnt) where TEvent : class, IEvent
        {
            Argument.IsNotNull(evnt, nameof(evnt));

            ApplyOnState(evnt);

            Changes.Add(evnt);
            Version++;
        }

        protected void ProduceError(string errorMessage)
        {
            throw new InvalidAggregateStateException(errorMessage);
        }
        
        private void ApplyOnState(object evnt)
        {
            MethodInfo action = FindStateMethod(evnt);
            action?.Invoke(State, new object[] { evnt }); //todo use cached expressions
        }

        private MethodInfo FindStateMethod(object evnt)
        {
            return typeof(TState).FindMethodBySinglePameter(evnt.GetType());
        }
    }
}