using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSalad.Dispatching;
using CQRSalad.EventSourcing;

namespace CQRSalad.Infrastructure
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly Dispatcher _dispatcher;

        public InMemoryEventBus(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class, IEvent
        {
            await _dispatcher.PublishAsync(@event);
        }

        public async Task PublishAsync(IEnumerable<IEvent> events)
        {
            foreach (var @event in events)
            {
                await _dispatcher.PublishAsync(@event);
            }
        }
    }
}