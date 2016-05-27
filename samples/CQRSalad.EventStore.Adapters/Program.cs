using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using EventStore.ClientAPI;

namespace CQRSalad.EventStore.Adapters
{
    class Program
    {
        static void Main(string[] args)
        {
            var connection = EventStoreConnection.Create(new IPEndPoint(IPAddress.Loopback, 1113));
            // Don't forget to tell the connection to connect!
            connection.ConnectAsync().Wait();

            var myEvent = new EventData(Guid.NewGuid(), "testEvent", false,
                                        Encoding.UTF8.GetBytes("some data"),
                                        Encoding.UTF8.GetBytes("some metadata"));

            connection.AppendToStreamAsync("test-stream", ExpectedVersion.Any, myEvent).Wait();

            StreamEventsSlice streamEvents = connection.ReadStreamEventsForwardAsync("test-stream", 0, 1, false).Result;

            RecordedEvent returnedEvent = streamEvents.Events[0].Event;

            Console.WriteLine("Read event with data: {0}, metadata: {1}",
                Encoding.UTF8.GetString(returnedEvent.Data),
                Encoding.UTF8.GetString(returnedEvent.Metadata));
        }
    }
}