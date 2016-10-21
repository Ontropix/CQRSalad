namespace CQRSalad.Infrastructure.Buses
{
    public interface IDomainBus : ICommandBus, IQueryBus
    {
    }
}