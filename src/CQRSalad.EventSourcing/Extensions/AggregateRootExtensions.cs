using System;
using System.Reflection;
using CQRSalad.Domain;

namespace CQRSalad.EventSourcing
{
    public static class AggregateRootExtensions
    {
        public static AggregateSnapshot MakeSnapshot(this AggregateRoot aggregate)
        {
            return new AggregateSnapshot()
            {
                AggregateId = aggregate.Id,
                AggregateType = aggregate.GetType(),
                Version = aggregate.Version,
                State = aggregate.GetState(),
                Timestamp = DateTime.UtcNow
            };
        }

        public static void RestoreFromSnapshot(this AggregateRoot aggregate, AggregateSnapshot snapshot)
        {
            aggregate.Id = snapshot.AggregateId;
            aggregate.Version = snapshot.Version;

            if (aggregate.HasState)
            {
                aggregate.SetState(snapshot.State);
            }
        }

        private static PropertyInfo GetStateProperty(this AggregateRoot aggregate)
        {
            return aggregate.GetType().GetProperty(nameof(AggregateRoot<object>.State), BindingFlags.NonPublic | BindingFlags.Instance);
        }

        private static object GetState(this AggregateRoot aggregate)
        {
            return aggregate.GetStateProperty().GetValue(aggregate);
        }

        private static void SetState(this AggregateRoot aggregate, object value)
        {
            aggregate.GetStateProperty().SetValue(aggregate, value);
        }

        public static TEvent MapToEvent<TEvent>(this ICommand command) where TEvent: class, IEvent, new()
        {
            TEvent evnt = new TEvent();
            evnt.InjectFromCommand(command);
            return evnt;
        }
    }
}
