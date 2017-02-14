using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSalad.Dispatching;
using CQRSalad.EventSourcing;

namespace CQRSalad.Infrastructure
{
    public class InMemoryCommandBus : ICommandBus
    {
        private readonly Dispatcher _dispatcher;
        private readonly IEventBus _eventBus;

        public InMemoryCommandBus(Dispatcher dispatcher, IEventBus eventBus)
        {
            _dispatcher = dispatcher;
            _eventBus = eventBus;
        }

        public async Task CommandAsync<TCommand>(TCommand command, string senderId) where TCommand : class, ICommand
        {
            IEnumerable<IEvent> events = (IEnumerable<IEvent>) await _dispatcher.SendAsync(command);
            await _eventBus.PublishAsync(events);
        }
    }
}