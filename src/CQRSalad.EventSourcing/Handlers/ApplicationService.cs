using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CQRSalad.EventStore.Core;

namespace CQRSalad.EventSourcing
{
    public abstract class ApplicationService<TAggregate> where TAggregate : AggregateRoot, new()
    {
        private readonly IAggregateRepository<TAggregate> _aggregateRepository;
        //private readonly IEventBus _eventBus;

        protected ApplicationService(IAggregateRepository<TAggregate> aggregateRepository, IEventBus eventBus)
        {
            Argument.IsNotNull(aggregateRepository, nameof(aggregateRepository));
            Argument.IsNotNull(eventBus, nameof(eventBus));

            _aggregateRepository = aggregateRepository;
            //_eventBus = eventBus;
        }

        public async Task Execute<TCommand>(TCommand command) where TCommand : class
        {
            Argument.IsNotNull(command, nameof(command));

            string aggregateId = GetAggregateId(command);
            TAggregate aggregate = await _aggregateRepository.LoadById(aggregateId);

            var context = new CommandExecutionContext<TCommand>(aggregate, command);
            context.Perform();
            
            await _aggregateRepository.Save(aggregate);
            //await _eventBus.Publish(aggregate.Changes);
        }

        private string GetAggregateId(object command)
        {
            var propertiesWithAggregateId = command
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(prop => prop.IsDefined(typeof(AggregateIdAttribute), false));

            //todo check count

            return (string) propertiesWithAggregateId.First().GetValue(command); //todo check type
        }
    }
}