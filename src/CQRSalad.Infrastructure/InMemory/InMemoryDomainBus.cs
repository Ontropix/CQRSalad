using System.Threading.Tasks;
using CQRSalad.EventSourcing;
using CQRSalad.Infrastructure.Validation;

namespace CQRSalad.Infrastructure
{
    public class InMemoryDomainBus : IDomainBus
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly ValidationController _validationController;

        public InMemoryDomainBus(ICommandBus commandBus, IQueryBus queryBus, ValidationController validationController)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _validationController = validationController;
        }

        public async Task CommandAsync<TCommand>(TCommand command, string senderId) where TCommand : class, ICommand
        {
            ValidateMessage(command);
            await _commandBus.CommandAsync(command, senderId);
        }

        public async Task<TResult> QueryAsync<TResult>(IQueryFor<TResult> query, string senderId)
        {
            ValidateMessage(query);
            return await _queryBus.QueryAsync(query, senderId);
        }

        protected void ValidateMessage<TMessage>(TMessage message) where TMessage : class
        {
            _validationController.Validate(message);
        }
    }
}