using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using CQRSalad.EventSourcing;
using CQRSalad.EventSourcing.Specifications;
using Newtonsoft.Json;

namespace CQRSalad.Infrastructure
{
    public static class SpecificationRunner
    {
        public static void Run<TAggregate>(AggregateSpecification<TAggregate> specification, TextWriter writer)
            where TAggregate : IAggregateRoot, new()
        {
            SpecificationResult result = specification.Verify();

            try
            {
                if (result.Obtained.Count != result.Expected.Count)
                {
                    throw new UnexpectedEventException("");
                }

                for (var index = 0; index < result.Obtained.Count; index++)
                {
                    object expected = result.Expected[index];
                    object obtained = result.Obtained[index];

                    if (expected.GetType() != obtained.GetType())
                    {
                        throw new UnexpectedEventException("");
                    }

                    if (!CompareEvents(expected, obtained))
                    {
                        throw new UnexpectedEventException("");
                    }
                }
            }
            finally
            {
                writer.Write(DumpSpecification(specification.GetType(), result));
            }
        }

        private static bool CompareEvents(object one, object two)
        {
            return string.Equals(JsonConvert.SerializeObject(one), JsonConvert.SerializeObject(two),
                StringComparison.Ordinal);
        }
        
        private static string DumpSpecification(Type specType, SpecificationResult result)
        {
            var output = new StringBuilder();

            output.AppendLine($"Specification: { specType.AssemblyQualifiedName }\n");

            output.DumpList("Given", result.Given);
            output.DumpList("Expected", result.Expected);
            output.DumpList("Obtained", result.Obtained);

            return output.ToString();
        }

        private static void DumpList(this StringBuilder builder, string name, IEnumerable<IEvent> list)
        {
            builder.AppendLine($"{name}:");
            builder.AppendLine("[");
            foreach (var item in list)
            {
                builder.Append($"{DumpObject(item)} \r\n");
            }
            builder.AppendLine("]");
        }

        private static string DumpObject(object @event)
        {
            string json = JsonConvert.SerializeObject(@event, Formatting.Indented);
            return $"\t//Type: {@event.GetType().AssemblyQualifiedName}" +
                   $"\n" +
                   $"\t{json.Replace("\n", "\t")}";
        }
    }
}