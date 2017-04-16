using System;
using System.Linq;
using System.Threading.Tasks;
using CQRSalad.EventSourcing;

namespace CQRSalad.EventStore.Core
{
    public class ShapshotAggregateRepository<TAggregate> : AggregateRepository<TAggregate>
        where TAggregate : class, IAggregateRoot, new()
    {
        private readonly IEventStoreAdapter _eventStore;
        private readonly ISnapshotStore _snapshotStore;
        private readonly int _makeSnapshotOnVersion;

        public ShapshotAggregateRepository(IEventStoreAdapter eventStore, ISnapshotStore snapshotStore, int makeSnapshotOnVersion) 
            : base(eventStore)
        {
            Argument.IsNotNull(eventStore, nameof(eventStore));
            Argument.IsNotNull(snapshotStore, nameof(snapshotStore));
            Argument.NotNegative(makeSnapshotOnVersion, nameof(makeSnapshotOnVersion));

            _eventStore = eventStore;
            _snapshotStore = snapshotStore;
            _makeSnapshotOnVersion = makeSnapshotOnVersion;
        }

        public override async Task<TAggregate> LoadById(string aggregateId)
        {
            Argument.StringNotEmpty(aggregateId, nameof(aggregateId));
            
            AggregateSnapshot snapshot = await _snapshotStore.LoadSnapshot(aggregateId);
            if (snapshot == null)
            {
                return await base.LoadById(aggregateId);
            }

            var aggregate = new TAggregate();
            aggregate.Restore(snapshot);
            
            var stream = await _eventStore.GetStreamAsync(aggregateId, snapshot.Version + 1);
            aggregate.Reel(stream.ToList());
            return aggregate;
        }

        public override async Task Save(TAggregate aggregate)
        {
            await base.Save(aggregate);

            if (aggregate.Version > 0 && aggregate.Version % _makeSnapshotOnVersion == 0)
            {
                AggregateSnapshot snapshot = aggregate.MakeSnapshot();
                await _snapshotStore.SaveSnapshot(snapshot);
            }
        }
    }
}