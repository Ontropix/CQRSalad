using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSalad.Dispatching.Core;
using CQRSalad.Domain;

namespace CQRSalad.Infrastructure
{
    public class InMemoryCommandBus : ICommandBus
    {
        private readonly IMessageDispatcher _dispatcher;

        public InMemoryCommandBus(IMessageDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task SendAsync<TCommand>(TCommand command, string senderId) where TCommand : class
        {
            await _dispatcher.SendAsync(command); //todo mock
        }
    }

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

    public class InMemoryDomainBus : IDomainBus
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;

        public InMemoryDomainBus(ICommandBus commandBus, IQueryBus queryBus)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
        }

        public async Task SendAsync<TCommand>(TCommand command, string senderId) where TCommand : class
        {
            await _commandBus.SendAsync(command, senderId);
        }

        public async Task<TResult> QueryAsync<TResult>(IQueryFor<TResult> query, string senderId)
        {
            return await _queryBus.QueryAsync(query, senderId);
        }
    }
}
