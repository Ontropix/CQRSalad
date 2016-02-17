using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSalad.EventStore.Core
{
    public interface IEventBus
    {
        Task Publish(IEvent @event);
        Task Publish(List<IEvent> events);
    }
}