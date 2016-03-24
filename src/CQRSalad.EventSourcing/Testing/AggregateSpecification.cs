using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CQRSalad.Dispatching.Async;
using CQRSalad.Domain;
using CQRSalad.EventSourcing.Testing.Exceptions;
using CQRSalad.EventSourcing.Testing.Extensions;
using CQRSalad.EventStore.Core;
using ServiceStack;
using ServiceStack.Text;

namespace CQRSalad.EventSourcing.Testing
{
    public class AggregateSpecification<TAggregate> where TAggregate : AggregateRoot, new()
    {
        private readonly AggregateRepository<TAggregate> _aggregateRepository;
        private List<IEvent> ObtainedEvents { get; set; }

        protected IIdGenerator IdGenerator { get; }

        public AggregateSpecification(AggregateRepository<TAggregate> aggregateRepository, IIdGenerator idGenerator)
        {
            JsConfig.ExcludeTypes.Add(typeof(MessageMetadata));
            
            _aggregateRepository = aggregateRepository;
            IdGenerator = idGenerator;
        }

        public async Task Given(List<IEvent> givenEvents)
        {
            if (givenEvents.Count == 0)
            {
                return;
            }

            CheckAggregateId(givenEvents);

            string aggregateId = givenEvents[0].AggregateId;
            TAggregate aggregate = await _aggregateRepository.LoadById(aggregateId);

           // aggregate.Reel(givenEvents); //todo

            await _aggregateRepository.Save(aggregate);
        }

        public async Task When<TCommand>(TCommand command) where TCommand : class
        {
            //var context = new CommandExecutionContext();

            //ObtainedEvents = await _commandProcessor.Process(command.AggregateId, command);
        }

        public async Task Expected(params IEvent[] expectedEvents)
        {
            await Task.Run(() =>
            {
                if (expectedEvents == null || expectedEvents.Length < 1)
                {
                    throw new ArgumentException("No expected events provided.");
                }

                CheckAggregateId(expectedEvents);

                if (ObtainedEvents.Count != expectedEvents.Length)
                {
                    string expectedJson = $"Expected:\r{String.Join("\r", expectedEvents.Select(t => t.GetType().FullName))}";
                    string obtainedJson = $"Got:\r{String.Join("\r", ObtainedEvents.Select(t => t.GetType().FullName))}";
                    throw new UnexpectedEventException($"\r\n{expectedJson}\r\n{obtainedJson}");
                }

                for (var index = 0; index < ObtainedEvents.Count; index++)
                {
                    IEvent expected = expectedEvents[index];
                    IEvent obtained = ObtainedEvents[index];

                    if (expected.GetType() != obtained.GetType())
                    {

                        string error = $"Unexpected event. \r\nExpected: \r{Dump(expected)} \r\nGot: \r{Dump(obtained)}";
                        throw new UnexpectedEventException(error);
                    }

                    if (!CompareEvents(expected, obtained))
                    {
                        string error = $"Events are not match. \r\nExpected: \r{Dump(expected)} \r\nGot: \r{Dump(obtained)}";
                        throw new EventsNotMatchException(error);
                    }
                }
            });
        }

        private static bool CompareEvents(IEvent one, IEvent two)
        {
            return String.Equals(one.ToJson(), two.ToJson(), StringComparison.Ordinal);
        }

        private static string Dump(IEvent @event)
        {
            return @event.Dump().Replace("\n", "");
        }

        private static void CheckAggregateId(IEnumerable<IEvent> events)
        {
            if (!events.AllEqual(@event => @event.AggregateId))
            {
                throw new ArgumentException("All 'given' events should have the same AggregateId.");
            }
        }
    }
}
