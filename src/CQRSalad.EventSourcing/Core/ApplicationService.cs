using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using CQRSalad.Domain;

namespace CQRSalad.EventSourcing
{
    public abstract class ApplicationService<TAggregate> where TAggregate : AggregateRoot, new()
    {
        private readonly IAggregateRepository<TAggregate> _aggregateRepository;
        private CommandExecutorsManager ExecutorsManager { get; } = new CommandExecutorsManager();

        protected ApplicationService(IAggregateRepository<TAggregate> aggregateRepository)
        {
            Argument.IsNotNull(aggregateRepository, nameof(aggregateRepository));
            _aggregateRepository = aggregateRepository;
        }

        public async Task<List<IEvent>> Execute<TCommand>(TCommand command) where TCommand : class, ICommand
        {
            Argument.IsNotNull(command, nameof(command));

            string aggregateId = GetAggregateId(command);
            TAggregate aggregate = await _aggregateRepository.LoadById(aggregateId);
            
            var context = new DomainContext(aggregate, command);
            CommandExecutor executor = ExecutorsManager.GetExecutor(typeof (TAggregate), typeof(TCommand));
            executor(context);

            if (action == null)
            {
                throw new CommandProcessingException("Aggregate doesn't handle command.");
            }

            var ctor = action.GetCustomAttribute<AggregateCtorAttribute>(false);

            if (ctor == null && Aggregate.Version == 0)
            {
                throw new InvalidOperationException("Attempting to create an aggregate using non-constructor command.");
            }

            if (ctor != null && Aggregate.Version > 0)
            {
                throw new InvalidOperationException("Attempting to create existed aggregate.");
            }

            action.Invoke(Aggregate, new object[] { Command });

            if (Aggregate.Changes.Count < 1)
            {
                throw new CommandProducedNoEventsException(Command);
            }

            await _aggregateRepository.Save(aggregate);
            return aggregate.Changes;
        }

        private string GetAggregateId(object command)
        {
            //TODO cache
            var propertiesWithAggregateId = command
                .GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                .Where(prop => prop.IsDefined(typeof(AggregateIdAttribute), false));

            //todo check count

            return (string) propertiesWithAggregateId.First().GetValue(command); //todo check type
        }
    }
}