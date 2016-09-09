using System.Threading.Tasks;
using CQRSalad.Dispatching.NEW.Core;
using CQRSalad.Domain;

namespace CQRSalad.Infrastructure
{
    public class InMemoryCommandBus : ICommandBus
    {
        private readonly Dispatcher _dispatcher;

        public InMemoryCommandBus(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task<CommandResult> SendAsync<TCommand>(TCommand command, string senderId) where TCommand : class
        {
            return await _dispatcher.SendAsync(command);
        }
    }

    public class InMemoryQueryBus : IQueryBus
    {
        private readonly Dispatcher _dispatcher;

        public InMemoryQueryBus(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task<QueryResult<TResult>> QueryAsync<TResult>(IQueryFor<TResult> query, string senderId)
        {
            object result = await _dispatcher.SendAsync(query);
            return  new QueryResult<TResult>
            {
                
            };
        }
    }
}
