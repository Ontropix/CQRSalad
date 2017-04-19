using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public interface ISnapshotStore
    {
        Task<AggregateSnapshot> GetSnapshot(string aggregateId);
        Task SaveSnapshot(AggregateSnapshot snapshot);
    }
}