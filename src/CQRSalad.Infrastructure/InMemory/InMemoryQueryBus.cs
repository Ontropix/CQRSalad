using System.Threading.Tasks;
using CQRSalad.Dispatching;
using CQRSalad.EventSourcing;

namespace CQRSalad.Infrastructure
{
    public class InMemoryQueryBus : IQueryBus
    {
        private readonly Dispatcher _dispatcher;

        public InMemoryQueryBus(Dispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task<TResult> QueryAsync<TResult>(IQueryFor<TResult> query, string senderId)
        {
            object result = await _dispatcher.SendAsync(query);
            return (TResult) result;
        }
    }
}