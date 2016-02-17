namespace CQRSalad.EventStore.MongoDB
{
    public class EventStoreSettings
    {
        public string StreamsesCollectionName { get; set; }

        public EventStoreSettings(string streamsCollectionName)
        {
            StreamsesCollectionName = streamsCollectionName;
        }

        public static EventStoreSettings GetDefault()
        {
            return new EventStoreSettings("streams");
        }
    }
}