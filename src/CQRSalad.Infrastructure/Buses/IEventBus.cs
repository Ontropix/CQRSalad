using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSalad.EventSourcing;

namespace CQRSalad.Infrastructure
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent;
        Task PublishAsync(IEnumerable<IEvent> events);
    }
}