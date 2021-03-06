﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CQRSalad.EventSourcing
{
    public abstract class ApplicationService<TAggregate> where TAggregate : IAggregateRoot, new()
    {
        private readonly IAggregateRepository<TAggregate> _aggregateRepository;

        protected ApplicationService(IAggregateRepository<TAggregate> aggregateRepository)
        {
            Argument.IsNotNull(aggregateRepository, nameof(aggregateRepository));
            _aggregateRepository = aggregateRepository;
        }

        public async Task<IEnumerable<IEvent>> Process<TCommand>(TCommand command)
            where TCommand : class, ICommand
        {
            Argument.IsNotNull(command, nameof(command));

            string aggregateId = command.GetAggregateId();
            TAggregate aggregate = await _aggregateRepository.LoadById(aggregateId);

            aggregate.Perform(command);

            await _aggregateRepository.Save(aggregate);
            return aggregate.Changes;
        }
    }
}