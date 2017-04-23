using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public class InMemorySnapshotStore : ISnapshotStore
    {
        private readonly ConcurrentDictionary<string, AggregateSnapshot> _snapshots = 
            new ConcurrentDictionary<string, AggregateSnapshot>();

        public Task<AggregateSnapshot> GetSnapshot(string aggregateId)
        {
            AggregateSnapshot snapshot;
            _snapshots.TryGetValue(aggregateId, out snapshot);
            return Task.FromResult(snapshot);
        }

        public Task SaveSnapshot(AggregateSnapshot snapshot)
        {
            _snapshots.AddOrUpdate(snapshot.AggregateId, snapshot, (key, value) =>
            {
                value.State = snapshot.State;
                value.Version = snapshot.Version;
                return value;
            });
            return Task.CompletedTask;
        }
    }
}