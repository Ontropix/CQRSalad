using Kutcha.Core;
using Kutcha.InMemory;
using StructureMap;

namespace Samples.Configuration.Configurators
{
    public static class TransportConfigurator
    {
        public static IContainer UseInMemoryBuses(this IContainer container)
        {
            container.Configure(expression => expression.For<ICommandBus>().Use<InMemoryDomainBus>().Singleton());
            container.Configure(expression => expression.For<IQueryBus>().Use<InMemoryDomainBus>().Singleton());
            //container.Configure(expression => expression.For<IDomainBus>().Use<InMemoryDomainBus>().Singleton());
            //container.Configure(expression => expression.Policies.FillAllPropertiesOfType<IQueryBus>().Use(new InMemoryQueryBus(dispatcher)).Singleton());
            return container;
        }

        public static IContainer UseGuidIdGenerator(this IContainer container)
        {
            IIdGenerator idGenerator = new GuidIdGenerator();
            container.Configure(expression => expression.For<IIdGenerator>().Use(idGenerator));
            return container;
        }

        public static IContainer UseMongoIdGenerator(this IContainer container)
        {
            IIdGenerator idGenerator = new MongoIdGenerator();
            container.Configure(expression => expression.For<IIdGenerator>().Use(idGenerator));
            return container;
        }

        public static IContainer UseInMemoryKutcha(this IContainer container)
        {
            container.Configure(config => config.For<IKutchaContext>().Use<InMemoryKutchaContext>().Singleton());
            return container;
        }
    }
}