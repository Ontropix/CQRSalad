using System.Threading.Tasks;
using CQRSalad.Dispatching.Core;
using CQRSalad.Domain;
using CQRSalad.Infrastructure.Buses;

namespace CQRSalad.Infrastructure
{
    public class InMemoryQueryBus : IQueryBus
    {
        private readonly IMessageDispatcher _dispatcher;

        public InMemoryQueryBus(IMessageDispatcher dispatcher)
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