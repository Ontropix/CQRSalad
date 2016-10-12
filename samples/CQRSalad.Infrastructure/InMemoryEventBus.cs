using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSalad.Dispatching.Core;
using CQRSalad.Domain;

namespace CQRSalad.Infrastructure
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly IMessageDispatcher _dispatcher;

        public InMemoryEventBus(IMessageDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task PublishAsync<TEvent>(TEvent @event) where TEvent : class
        {
            await _dispatcher.PublishAsync(@event);
        }

        public async Task PublishAsync(List<object> events)
        {
            foreach (var @event in events)
            {
                await _dispatcher.PublishAsync(@event);
            }
        }
    }
}