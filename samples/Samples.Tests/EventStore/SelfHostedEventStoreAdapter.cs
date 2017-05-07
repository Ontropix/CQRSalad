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

        public int FirstEventIndex => 0;

        public async Task CreateStreamAsync(string streamId, EventStreamMetadata meta)
        {
            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();

                var metadata = StreamMetadata
                        .Build()
                        .SetCustomProperty("AggregateRoot", meta.AggregateRootType.AssemblyQualifiedName)
                        //.SetCustomProperty("StartedOn", meta.StartedOn)
                        ;
                var result = await connection.SetStreamMetadataAsync(streamId, ExpectedVersion.NoStream, metadata);
            }
        }

        public async Task<EventStream> GetStreamAsync(string streamId)
        {
            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();

                var meta = await connection.GetStreamMetadataAsync(streamId);
                bool isEndOfStream;
                meta.StreamMetadata.TryGetValue("IsEnded", out isEndOfStream);

                var firstSlice = await connection.ReadStreamEventsForwardAsync(streamId, StreamStartIndex, StreamSliceSize, false);
                var stream = new EventStream
                {
                    StreamId = streamId,
                    Version = (int)firstSlice.LastEventNumber, //todo
                    Events = firstSlice.Events.Select(x => DeserializeEvent(x.Event)).ToList(),
                    RootStatus = GetStatus(firstSlice.LastEventNumber, isEndOfStream)
                };

                return stream;
            }
        }

        public async Task<EventStream> GetStreamAsync(string streamId, int fromVersion, int toVersion = -1)
        {
            if (aggregate.Status == AggregateStatus.New)
            {
                await _eventStore.CreateStreamAsync(
                    aggregate.Id,
                    new EventStreamMetadata
                    {
                        AggregateRootType = typeof(TAggregate),
                        StartedOn = DateTime.UtcNow
                    }
                );
            }

            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();
                var stream = await connection.ReadStreamEventsForwardAsync(streamId, fromVersion, toVersion == -1 ? StreamSliceSize : toVersion, false);
                var meta = await connection.GetStreamMetadataAsync(streamId);

                bool isEndOfStream;
                meta.StreamMetadata.TryGetValue("IsEnded", out isEndOfStream);

                return new EventStream
                {
                    StreamId = streamId,
                    Version = (int)stream.LastEventNumber,//todo
                    Events = stream.Events.Select(x => DeserializeEvent(x.Event)).ToList(),
                    RootStatus = GetStatus(stream.LastEventNumber, isEndOfStream)
                };
            }

            if (aggregate.Status == AggregateStatus.Archived)
            {
                await _eventStore.MarkStreamAsEnded(aggregate.Id);

                var metadata = StreamMetadata
                    .Build()
                    .SetCustomProperty("IsEnded", true);
                var result = await connection.SetStreamMetadataAsync(streamId, ExpectedVersion.StreamExists, metadata);
            }
        }

        private AggregateStatus GetStatus(long streamVersion, bool isEnded)
        {
            if (streamVersion >= StreamStartIndex && !isEnded)
            {
                return AggregateStatus.Alive;
            }

            if (streamVersion >= StreamStartIndex && isEnded)
            {
                return AggregateStatus.Archived;
            }

            if (streamVersion < StreamStartIndex && !isEnded)
            {
                return AggregateStatus.New;
            }

            throw new InvalidOperationException("RootStatus!");
        }

        public async Task AppendEventsAsync(string streamId, IEnumerable<IEvent> events, int expectedVersion)
        {
            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();

                var eventData = events.Select(x => new EventData(
                    eventId: GenerateId(),
                    type: x.GetType().AssemblyQualifiedName,
                    isJson: true,
                    data: SerializeEvent(x),
                    metadata: null
                ));
                
                var result = await connection.AppendToStreamAsync(
                    streamId,
                    expectedVersion,
                    eventData
                    );
            }
        }
        
        private IEvent DeserializeEvent(RecordedEvent recoredEvent)
        {
            Type eventType = Type.GetType(recoredEvent.EventType);
            string dataJson = Encoding.UTF8.GetString(recoredEvent.Data);
            return (IEvent)JsonConvert.DeserializeObject(dataJson, eventType);
        }

        private byte[] SerializeEvent(IEvent evnt)
        {
            string json = JsonConvert.SerializeObject(evnt);
            return Encoding.UTF8.GetBytes(json);
        }

        private Guid GenerateId()
        {
            return Guid.NewGuid();
        }
    }
}
