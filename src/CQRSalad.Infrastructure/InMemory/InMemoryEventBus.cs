using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSalad.Dispatching.Core;
using CQRSalad.EventSourcing;

namespace CQRSalad.Infrastructure
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly IMessageDispatcher _dispatcher;

        public InMemoryEventBus(IMessageDispatcher dispatcher)
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