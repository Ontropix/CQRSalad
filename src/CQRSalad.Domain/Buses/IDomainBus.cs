namespace CQRSalad.Domain
{
    public interface IDomainBus : ICommandBus, IQueryBus
    {
    }
}