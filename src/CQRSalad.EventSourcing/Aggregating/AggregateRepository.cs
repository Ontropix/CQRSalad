using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSalad.Domain;
using CQRSalad.EventStore.Core;

namespace CQRSalad.EventSourcing
{
    public class AggregateRepository<TAggregate> : IAggregateRepository<TAggregate>
        where TAggregate : AggregateRoot, new()
    {
        private readonly IEventStore _eventStore;
        private readonly IIdGenerator _idGenerator;

        public AggregateRepository(IEventStore eventStore, IIdGenerator idGenerator)
        {
            Argument.IsNotNull(eventStore, nameof(eventStore));
            _eventStore = eventStore;
            _idGenerator = idGenerator;
        }

        public virtual async Task<TAggregate> LoadById(string aggregateId)
        {
            Argument.StringNotEmpty(aggregateId, nameof(aggregateId));

            //todo has state optimization?
            List<DomainEvent> stream = await _eventStore.GetStreamAsync(aggregateId);

            var aggregate = new TAggregate { Id = aggregateId };
            aggregate.Reel(stream.Select(x => x.Body).ToList()); //todo

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

            //add metadata
            DateTime currentTime = DateTime.UtcNow;
            var domainEvents = aggregate.Changes.Select(x => new DomainEvent
            {
                EventId = _idGenerator.Generate(),
                Body = x,
                Meta = new EventMetadata
                {
                    AggregateId = aggregate.Id,
                    AggregateRoot = GetType().AssemblyQualifiedName,
                    Timestamp = currentTime
                }
            }).ToList();

            await _eventStore.AppendManyAsync(aggregate.Id, domainEvents);
        }
    }
}