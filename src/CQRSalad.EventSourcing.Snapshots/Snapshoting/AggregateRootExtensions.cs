using System;
using CQRSalad.EventStore.Core;

namespace CQRSalad.EventSourcing
{
    internal static class AggregateRootExtensions
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
    }
}
