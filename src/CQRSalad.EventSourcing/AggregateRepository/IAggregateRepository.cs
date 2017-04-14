using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public interface IAggregateRepository<TAggregate> where TAggregate : IAggregateRoot, new()
    {
        Task<TAggregate> LoadById(string aggregateId);
        Task Save(TAggregate aggregate);
    }
}