using System;
using CQRSalad.EventSourcing;

namespace CQRSalad.EventStore.Core
{
    internal static class IAggregateRootExtensions
    {
        internal static AggregateSnapshot MakeSnapshot(this IAggregateRoot aggregate)
        {
            return new AggregateSnapshot
            {
                AggregateId = aggregate.Id,
                AggregateType = aggregate.GetType(),
                Version = aggregate.Version,
                State = aggregate.State,
                Timestamp = DateTime.UtcNow
            };
        }

        internal static void Restore(this IAggregateRoot aggregate, AggregateSnapshot snapshot)
        {
            aggregate.Id = snapshot.AggregateId;
            aggregate.State = snapshot.State;
            aggregate.Version = snapshot.Version;
        }
    }
}