using System;
using CQRSalad.Domain;

namespace CQRSalad.EventSourcing
{
    internal class DomainContext
    {
        protected AggregateRoot Aggregate { get; }
        protected ICommand Command { get; }

        public DomainContext(AggregateRoot aggregate, ICommand command)
        {
            Argument.IsNotNull(aggregate, nameof(aggregate));
            Argument.IsNotNull(command, nameof(command));

            Aggregate = aggregate;
            Command = command;
        }
    }
}