using System;
using System.Threading.Tasks;
using CQRSalad.Domain;

namespace CQRSalad.EventSourcing
{
    public abstract class WorkflowService
    {
        private readonly IDomainBus _domainBus;

        protected WorkflowService(IDomainBus domainBus)
        {
            _domainBus = domainBus;
        }

        protected async Task ProduceCommandAsync<TCommand>(TCommand command, string sender) where TCommand : class
        {
            Argument.IsNotNull(command, nameof(command));
            Argument.StringNotEmpty(sender, nameof(sender));

            await _domainBus.SendAsync(command, sender);
        }

        protected async Task<TResult> QueryAsync<TResult>(IQueryFor<TResult> query, string sender)
        {
            Argument.IsNotNull(query, nameof(query));
            Argument.StringNotEmpty(sender, nameof(sender));

            return await _domainBus.QueryAsync(query, sender);
        }
    }
}