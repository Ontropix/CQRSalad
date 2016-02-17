namespace CQRSalad.EventStore.Core
{
    public interface IEvent
    {
        string AggregateId { get; }
    }
}