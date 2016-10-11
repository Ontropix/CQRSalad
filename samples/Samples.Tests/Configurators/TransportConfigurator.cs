using System;
using CQRSalad.Domain;
using CQRSalad.Infrastructure;
using Kutcha.Core;
using Kutcha.InMemory;
using StructureMap;

namespace Samples.Tests.Configurators
{
    public static class TransportConfigurator
    {
        public static IContainer UseInMemoryBuses(this IContainer container)
        {
            container.Configure(expression => expression.For<ICommandBus>().Use<InMemoryCommandBus>().Singleton());
            container.Configure(expression => expression.For<IQueryBus>().Use<InMemoryQueryBus>().Singleton());

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

        public static IContainer UseInMemoryKutcha(this IContainer container)
        {
            container.Configure(config => config.For<IKutchaContext>().Use<InMemoryKutchaContext>().Singleton());
            return container;
        }
    }

    public class GuidIdGenerator : IIdGenerator
    {
        public string Generate()
        {
            return Guid.NewGuid().ToString();
        }
    }
}