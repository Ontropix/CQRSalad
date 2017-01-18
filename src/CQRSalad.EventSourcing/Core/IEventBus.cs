using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
        Task PublishAsync(IEnumerable<IEvent> events);
    }
}