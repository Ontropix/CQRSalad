using System.Threading.Tasks;
using CQRSalad.Domain;
using CQRSalad.Infrastructure.Validation;

namespace CQRSalad.Infrastructure
{
    public class InMemoryDomainBus : IDomainBus
    {
        private readonly ICommandBus _commandBus;
        private readonly IQueryBus _queryBus;
        private readonly IMessageValidationFacade _validationFacade;

        public InMemoryDomainBus(ICommandBus commandBus, IQueryBus queryBus, IMessageValidationFacade validationFacade)
        {
            _commandBus = commandBus;
            _queryBus = queryBus;
            _validationFacade = validationFacade;
        }

        public async Task CommandAsync<TCommand>(TCommand command, string senderId) where TCommand : class
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
            _validationFacade.Validate(message);
        }
    }
}