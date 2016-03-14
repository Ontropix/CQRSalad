using System.Reflection;
using CQRSalad.Dispatching;
using CQRSalad.Dispatching.Async;
using CQRSalad.EventSourcing;
using Samples.Configuration.Structuremap;
using Samples.Domain.Events;
using Samples.Domain.Events.User;
using Samples.View;
using StructureMap;

namespace Samples.Configuration.Configurators
{
    public static class DispatcherConfigurator
    {
        public static IContainer UseAsyncDispatcherSingleton(this IContainer container)
        {
            Assembly assemblyWithAggregates = ApplicationServiceGenerator.Generate(typeof(UserAggregate).Assembly);

            AsyncMessageDispatcher dispatcher = DispatcherFactory.CreateAsync(configuration =>
            {
                configuration.SetServiceLocator(new StructureMapServiceProvider(container));
                configuration.AddScanRule(assemblyWithAggregates); //scan for command handlers
                configuration.AddScanRule(typeof(Event).Assembly); //scan for command handlers
                configuration.AddScanRule(typeof(_namespace).Assembly); //scan for event and query handlers
                configuration.EnableHandlingPriority();
                //configuration.AddInterceptor<LoggingInterceptor>();
            });

            container.Configure(expression => expression.For<AsyncMessageDispatcher>().Use(dispatcher).Singleton());
            return container;
        }
    }
}