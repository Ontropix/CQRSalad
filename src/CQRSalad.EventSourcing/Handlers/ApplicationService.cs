using System;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public abstract class ApplicationService<TAggregate> where TAggregate : AggregateRoot, new()
    {
        private readonly IAggregateRepository<TAggregate> _aggregateRepository;

        protected ApplicationService(IAggregateRepository<TAggregate> aggregateRepository)
        {
            Argument.IsNotNull(aggregateRepository, nameof(aggregateRepository));
            _aggregateRepository = aggregateRepository;
        }

        public async Task Execute<TCommand>(TCommand command) where TCommand : class
        {
            Argument.IsNotNull(command, nameof(command));

            string aggregateId = GetAggregateId(command);
            TAggregate aggregate = await _aggregateRepository.LoadById(aggregateId);

            var context = new CommandExecutionContext<TCommand>(aggregate, command);
            context.Perform();
            
            await _aggregateRepository.Save(aggregate);
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