using System.Threading.Tasks;

namespace CQRSalad.EventStore.Core
{
    public interface ISnapshotStore
    {
        Task<AggregateSnapshot> LoadSnapshot(string aggregateId);
        Task SaveSnapshot(AggregateSnapshot snapshot);
    }
}