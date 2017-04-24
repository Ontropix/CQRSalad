using System;
using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public class AggregateRepository<TAggregate> : IAggregateRepository<TAggregate>
        where TAggregate : class, IAggregateRoot, new()
    {
        private readonly IEventStoreAdapter _eventStore;

        public AggregateRepository(IEventStoreAdapter eventStore)
        {
            Argument.IsNotNull(eventStore, nameof(eventStore));
            _eventStore = eventStore;
        }

        public virtual async Task<TAggregate> LoadById(string aggregateId)
        {
            Argument.StringNotEmpty(aggregateId, nameof(aggregateId));
            
            EventStream stream = await _eventStore.GetStreamAsync(aggregateId);
            var aggregate = new TAggregate { Id =  aggregateId };
            aggregate.Restore(stream); //todo
            return aggregate;
        }

        public virtual async Task Save(TAggregate aggregate)
        {
            Argument.IsNotNull(aggregate, nameof(aggregate));
            Argument.StringNotEmpty(aggregate.Id, nameof(aggregate.Id));

            if (aggregate.Changes.Count < 1)
            {
                throw new InvalidOperationException("Attempting to save aggregate without changes.");
            }

            await _eventStore.AppendEventsAsync(
                aggregate.Id,
                aggregate.Changes,
                aggregate.Version + 1 //todo
            );
        }
    }
}