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
            if(address == null) throw new ArgumentNullException(nameof(address));
            if (portNumber < 0) throw new ArgumentOutOfRangeException(nameof(portNumber));

            _address = address;
            _portNumber = portNumber;

            _connectionSettings = ConnectionSettings.Create().Build();
        }
        
        public async Task<EventStream> GetStreamAsync(string streamId)
        {
            return await GetStreamAsync(streamId, StreamStartIndex);
        }

        public async Task<EventStream> GetStreamAsync(string streamId, int fromVersion, int toVersion = -1)
        {
            if (String.IsNullOrWhiteSpace(streamId)) throw new ArgumentException("StreamId is null or empty.", nameof(streamId));
            if (fromVersion < 0) throw new ArgumentOutOfRangeException(nameof(fromVersion));
            if (toVersion < -1) throw new ArgumentOutOfRangeException(nameof(toVersion));

            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();
                var stream = await connection.ReadStreamEventsForwardAsync(
                    stream: streamId, 
                    start: fromVersion, 
                    count: toVersion == -1 ? StreamSliceSize : toVersion,  //todo ToVersion != Count !!!
                    resolveLinkTos: false); 
                
                return new EventStream
                {
                    StreamId = streamId,
                    Version = Convert.ToInt32(stream.LastEventNumber), //todo add validation
                    Events = stream.Events.Select(x => DeserializeEvent(x.Event)).ToList(),
                };
            }
        }
        
        public async Task AppendEventsAsync(string streamId, IEnumerable<object> events, int expectedVersion)
        {
            using (var connection = EventStoreConnection.Create(_connectionSettings, new IPEndPoint(_address, _portNumber)))
            {
                await connection.ConnectAsync();

                var eventData = events.Select(x =>
                    new EventData(
                        eventId: Guid.NewGuid(),
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
        
        private object DeserializeEvent(RecordedEvent recoredEvent)
        {
            Type eventType = Type.GetType(recoredEvent.EventType);
            string dataJson = Encoding.UTF8.GetString(recoredEvent.Data);
            return JsonConvert.DeserializeObject(dataJson, eventType);
        }

        private byte[] SerializeEvent(object evnt)
        {
            string json = JsonConvert.SerializeObject(evnt);
            return Encoding.UTF8.GetBytes(json);
        }

        /* 
        
                private const string IsStreamClosedKey = "IsStreamClosed";
         
                var meta = await connection.GetStreamMetadataAsync(streamId);
                bool isStreamClosed;
                meta.StreamMetadata.TryGetValue(IsStreamClosedKey, out isStreamClosed);


        ---------------------

                if (isEndOfStream)
                {
                    var metadata = StreamMetadata
                           .Build()
                           .SetCustomProperty(IsStreamClosedKey, true);
                    await connection.SetStreamMetadataAsync(streamId, ExpectedVersion.NoStream, metadata);
                }
         */
    }
}