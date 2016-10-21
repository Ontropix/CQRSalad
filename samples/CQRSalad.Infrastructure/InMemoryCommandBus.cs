using System.Threading.Tasks;
using CQRSalad.Dispatching.Core;
using CQRSalad.Domain;
using CQRSalad.Infrastructure.Buses;

namespace CQRSalad.Infrastructure
{
    public class InMemoryCommandBus : ICommandBus
    {
        private readonly IMessageDispatcher _dispatcher;

        public InMemoryCommandBus(IMessageDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task CommandAsync<TCommand>(TCommand command, string senderId) where TCommand : class, ICommand
        {
            await _dispatcher.SendAsync(command);
        }
    }
}
