using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public interface IAggregateRepository<TAggregate> where TAggregate : AggregateRoot, new()
    {
        Task<TAggregate> LoadById(string aggregateId);
        Task Save(TAggregate aggregate);
    }
}