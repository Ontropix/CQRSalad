using System;

namespace CQRSalad.EventSourcing
{
    internal static class SnapshottingExtensions
    {
        internal static AggregateSnapshot MakeSnapshot(this IAggregateRoot aggregate)
        {
            return new AggregateSnapshot
            {
                AggregateId = aggregate.Id,
                AggregateType = aggregate.GetType(),
                Version = aggregate.Version,
                State = aggregate.State,
            };
        }

        internal static void Restore(this IAggregateRoot aggregate, AggregateSnapshot snapshot)
        {
            if (aggregate.Id != snapshot.AggregateId)
            {
                throw new InvalidOperationException("Invalid aggregateId in snapshopt.");
            }

            if (aggregate.GetType() != snapshot.AggregateType)
            {
                throw new InvalidOperationException("Trying to restore aggregate with wrong snapshot type.");
            }

            if (aggregate.State.GetType() != snapshot.State.GetType())
            {
                throw new InvalidOperationException("Trying to restore aggregate with wrong snapshot state type.");
            }

            aggregate.State = snapshot.State;
            aggregate.Version = snapshot.Version;
        }
    }
}