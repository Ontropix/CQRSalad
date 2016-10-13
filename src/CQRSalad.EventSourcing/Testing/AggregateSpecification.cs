using System;
using System.Collections.Generic;
using System.Linq;
using CQRSalad.EventSourcing.Testing.Exceptions;
using Newtonsoft.Json;


namespace CQRSalad.EventSourcing.Testing
{
    public class AggregateSpecification<TAggregate> where TAggregate : AggregateRoot, new()
    {
        private List<object> ObtainedEvents { get; set; }
        protected TAggregate Aggregate { get; set; }

        public AggregateSpecification()
        {
            Aggregate = new TAggregate();
        }

        public void Given(params object[] givenEvents)
        {
            if (givenEvents.Length == 0)
            {
                return;
            }

            Aggregate.Reel(givenEvents.ToList()); //todo
        }

        public void When<TCommand>(TCommand command) where TCommand : class
        {
            Argument.IsNotNull(command, nameof(command));
            
            var context = new CommandExecutionContext<TCommand>(Aggregate, command);
            context.Perform();
            ObtainedEvents = Aggregate.Changes;
        }

        public void Expected(params object[] expectedEvents)
        {
            if (expectedEvents == null || expectedEvents.Length < 1)
            {
                throw new ArgumentException("No expected events provided.");
            }

            if (ObtainedEvents.Count != expectedEvents.Length)
            {
                string expectedJson = $"Expected:\r{String.Join("\r", expectedEvents.Select(t => t.GetType().FullName))}";
                string obtainedJson = $"Got:\r{String.Join("\r", ObtainedEvents.Select(t => t.GetType().FullName))}";
                throw new UnexpectedEventException($"\r\n{expectedJson}\r\n{obtainedJson}");
            }

            for (var index = 0; index < ObtainedEvents.Count; index++)
            {
                object expected = expectedEvents[index];
                object obtained = ObtainedEvents[index];

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

            Console.WriteLine("Expected:");
            Console.WriteLine("[");
            foreach (var expectedEvent in expectedEvents)
            {
                Console.Write($"{Dump(expectedEvent)} \r\n");
            }
            Console.WriteLine("]");

            Console.WriteLine($"\r\nGot:");
            Console.WriteLine("[");
            foreach (var expectedEvent in expectedEvents)
            {
                Console.Write($"{Dump(expectedEvent)} \r\n");
            }
            Console.WriteLine("]");
        }

        private static bool CompareEvents(object one, object two)
        {
            return string.Equals(JsonConvert.SerializeObject(one), JsonConvert.SerializeObject(two), StringComparison.Ordinal);
        }

        private static string Dump(object @event)
        {
            string json = JsonConvert.SerializeObject(@event, Formatting.Indented);
            return json.Replace("\n", "");
        }
    }
}
