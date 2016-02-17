using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public interface ISnapshotStore
    {
        Task<AggregateSnapshot> LoadSnapshot(string aggregateId);
        Task SaveSnapshot(AggregateSnapshot snapshot);
    }
}