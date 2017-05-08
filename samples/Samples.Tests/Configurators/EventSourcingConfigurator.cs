using System.Net;
using CQRSalad.EventSourcing;
using CQRSalad.Infrastructure;
using Samples.Tests.EventStore;
using StructureMap;

namespace Samples.Tests.Configurators
{
    public static class EventSourcingConfigurator
    {
        public static IContainer UseCommandProcessorSingleton(this IContainer container)
        {
            container.Configure(cfg =>
                cfg.For<ISnapshotStore>().Use<InMemorySnapshotStore>().Singleton());
            container.Configure(cfg =>
                cfg.For(typeof (IAggregateRepository<>))
                    .Use(typeof (ShapshotAggregateRepository<>))
                    .Ctor<int>().Is(2)
                    .Singleton());
            //container.Configure(expression => expression.For(typeof(IAggregateRepository<>)).Use(typeof(AggregateRepository<>)).Singleton());
            return container;
        }

        public static IContainer UseInMemoryEventStore(this IContainer container)
        {
            container
                .Configure(expression => expression.For(typeof(IEventStoreAdapter))
                .Use(typeof(InMemoryEventStore))
                .Singleton());

            container.Configure(expression => expression.For(typeof(IEventBus)).Use(typeof(InMemoryEventBus)).Singleton());
            return container;
        }

        public static IContainer UseRealEventStore(this IContainer container)
        {
            container
                .Configure(expression => expression.For(typeof(IEventStoreAdapter))
                .Use(typeof(SelfHostedEventStoreAdapter))
                .Ctor<IPAddress>().Is(IPAddress.Loopback)
                .Ctor<int>().Is(1113)
                .Singleton());

            container.Configure(expression => expression.For(typeof(IEventBus)).Use(typeof(InMemoryEventBus)).Singleton());
            return container;
        }

        public static IContainer UseMongoEventStore(this IContainer container, string connectionString)
        {
            //var mongoEvents = new MongoInstance(connectionString);
            //var eventStore = new StreamBasedEventStore(mongoEvents.GetDatabase(), EventStoreSettings.GetDefault());
            //container.Configure(expression => expression.For<IEventStoreAdapter>().Use(eventStore).Singleton());

            //var snapshotsStore = new MongoSnapshotStore(mongoEvents.GetDatabase(), new MongoSnapshotsOptions() { CollectionName = "snapshots"});
            //container.Configure(expression => expression.For<ISnapshotStore>().Use(snapshotsStore).Singleton());

            return container;
        }
    }
}