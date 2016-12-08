namespace CQRSalad.Infrastructure
{
    public interface IDomainBus : ICommandBus, IQueryBus
    {
    }
}