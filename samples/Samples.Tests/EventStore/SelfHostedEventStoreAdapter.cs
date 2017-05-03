using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using CQRSalad.EventSourcing;
using EventStore.ClientAPI;
using Newtonsoft.Json;

namespace Samples.Tests.EventStore
{
    // TODO add event handlers
    // TODO add logging
    // TODO pretty exceptions
    // TODO add paging for reading big streams
    public class SelfHostedEventStoreAdapter : IEventStoreAdapter
    {
        private readonly IPAddress _address;
        private readonly int _portNumber;
        private readonly ConnectionSettings _connectionSettings;
        private const int StreamStartIndex = 0;
        private const int StreamSliceSize = 4096;

        public SelfHostedEventStoreAdapter(IPAddress address, int portNumber)
        {
            _address = address;
            _portNumber = portNumber;

            _connectionSettings = ConnectionSettings
                .Create()
                //.EnableVerboseLogging()
                //.UseConsoleLogger()
                .Build();
        }

        public async Task CreateStreamAsync(string streamId, EventStreamMetadata meta)
        {
            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();

                var metadata = StreamMetadata
                        .Build()
                        .SetMaxAge(TimeSpan.MaxValue)
                        .SetCustomProperty("AggregateRoot", meta.AggregateRootType.AssemblyQualifiedName)
                        .SetCustomProperty("StartedOn", meta.StartedOn.ToString("G"));
                var result = await connection.SetStreamMetadataAsync(streamId, ExpectedVersion.EmptyStream, metadata);
            }
        }

        public async Task<EventStream> GetStreamAsync(string streamId)
        {
            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();
                var stream  = await connection.ReadStreamEventsForwardAsync(streamId, StreamStartIndex, StreamSliceSize, false);
                //var meta = await conn.GetStreamMetadataAsync(streamId);

                return new EventStream
                {
                    StreamId = streamId,
                    Version = (int) (stream.LastEventNumber < 0 ? 0 : stream.LastEventNumber),//todo
                    IsEnded = stream.Status == SliceReadStatus.StreamDeleted,
                    Events = stream.Events.Select(x => DeserializeEvent(x.Event)).ToList()
                };
            }
        }

        public async Task<EventStream> GetStreamAsync(string streamId, int fromVersion, int toVersion = -1)
        {
            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();
                var stream = await connection.ReadStreamEventsForwardAsync(streamId, fromVersion, toVersion == -1 ? StreamSliceSize : toVersion, false);
                //var meta = await conn.GetStreamMetadataAsync(streamId);

                return new EventStream
                {
                    StreamId = streamId,
                    Version = (int)(stream.LastEventNumber < 0 ? 0 : stream.LastEventNumber),//todo
                    IsEnded = stream.Status == SliceReadStatus.StreamDeleted,
                    Events = stream.Events.Select(x => DeserializeEvent(x.Event)).ToList()
                };
            }
        }

        public async Task AppendEventsAsync(string streamId, IEnumerable<IEvent> events, int expectedVersion)
        {
            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();
                var result = await connection.AppendToStreamAsync(
                    streamId,
                    expectedVersion,
                    events.Select(x => new EventData(
                        eventId: Guid.NewGuid(),
                        type: x.GetType().AssemblyQualifiedName,
                        isJson: true,
                        data: SerializeEvent(x),
                        metadata: SerializeMeta(x)
                    )));
            }
        }

        public async Task MarkStreamAsEnded(string streamId)
        {
            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();

                var metadata = StreamMetadata
                    .Build()
                    .SetCustomProperty("IsEnded", true);
                var result = await connection.SetStreamMetadataAsync(streamId, ExpectedVersion.StreamExists, metadata);
            }
        }

        public async Task DeleteStreamAsync(string streamId)
        {
            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();
                var result = await connection.DeleteStreamAsync(streamId, ExpectedVersion.Any, true);
            }
        }

        private IEvent DeserializeEvent(RecordedEvent recoredEvent)
        {
            string metaJson = Encoding.UTF8.GetString(recoredEvent.Metadata);
            EventMetadata meta = JsonConvert.DeserializeObject<EventMetadata>(metaJson);
            Type eventType = Type.GetType(meta.EventType);

            string dataJson = Encoding.UTF8.GetString(recoredEvent.Data);
            return (IEvent)JsonConvert.DeserializeObject(dataJson, eventType);
        }

        private byte[] SerializeEvent(IEvent evnt)
        {
            string json = JsonConvert.SerializeObject(evnt);
            return Encoding.UTF8.GetBytes(json);
        }

        private byte[] SerializeMeta(IEvent evnt)
        {
            string json = JsonConvert.SerializeObject(
                new EventMetadata
                {
                    EventType = evnt.GetType().AssemblyQualifiedName
                });

            return Encoding.UTF8.GetBytes(json);
        }
    }

    public class EventMetadata
    {
        [JsonProperty("CLR_Type")]
        public string EventType { get; set; }
    }
}
