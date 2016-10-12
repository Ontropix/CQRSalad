using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSalad.Domain
{
    public interface IEventBus
    {
        Task PublishAsync<TEvent>(TEvent @event) where TEvent : class;
        Task PublishAsync(List<object> events);
    }
}