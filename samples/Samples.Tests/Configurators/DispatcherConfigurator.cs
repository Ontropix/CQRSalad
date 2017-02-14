using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CQRSalad.Dispatching;
using CQRSalad.Dispatching.Priority;
using CQRSalad.Dispatching.TypesScanning;
using CQRSalad.EventSourcing;
using CQRSalad.Infrastructure.Interceptors;
using Newtonsoft.Json;
using Samples.Tests.Structuremap;
using StructureMap;

namespace Samples.Tests.Configurators
{
    public static class DispatcherConfigurator
    {
        public static IContainer UseDispatcher(this IContainer container)
        {
            Assembly applicationServices = ApplicationServiceGenerator.Generate(typeof(Domain.Model._namespace).Assembly);

            var assemblies = new List<Assembly>
            {
                applicationServices,
                typeof(Samples.Domain.Workflow._namespace).Assembly,
                typeof(Samples.View._namespace).Assembly,
                typeof(Samples.View.Querying._namespace).Assembly
            };

            var typeProvider = new AssemblyTypesProvider(assemblies);
            container.Configure(expression => expression.For<IDispatcherTypesProvider>().Use(typeProvider).Singleton());

            var priorityProvider = new DefaultDispatcherPriorityProvider();
            container.Configure(expression => expression.For<IDispatcherPriorityProvider>().Use(priorityProvider).Singleton());

            Dispatcher dispatcher = Dispatcher.Create(configuration =>
            {
                configuration.SetServiceProvider(new StructureMapServiceProvider(container));

                var subscriptionManager = container.GetInstance<DispatcherSubscriptionsManager>();
                configuration.SetSubscriptionStore(subscriptionManager.Subscribe());

                configuration.AddInterceptor<ConsoleInterceptor>();
            });

            container.Configure(expression => expression.For<Dispatcher>().Use(dispatcher).Singleton());

            return container;
        }

        public class ConsoleQueriesInterceptor : QueriesInterceptor
        {
            public override async Task OnExecuting(DispatchingContext context)
            {
                await Task.CompletedTask;
                Console.WriteLine($"MESSAGE TYPE     : {context.MessageInstance.GetType()}");
                Console.WriteLine($"MESSAGE INSTANCE : {JsonConvert.SerializeObject(context.MessageInstance)}");
                Console.WriteLine($"HANDLER TYPE     : {context.HandlerInstance.GetType()}");
            }

            public override async Task OnExecuted(DispatchingContext context)
            {
                await Task.CompletedTask;
                Console.WriteLine($"RESULT: {JsonConvert.SerializeObject(context.Result)}");
                Console.WriteLine();
            }

            public override async Task OnException(DispatchingContext context)
            {
                await Task.CompletedTask;
            }
        }

        public class ConsoleInterceptor : IContextInterceptor
        {
            public async Task OnExecuting(DispatchingContext context)
            {
                await Task.CompletedTask;
                Console.WriteLine($"MESSAGE TYPE     : {context.MessageInstance.GetType()}");
                Console.WriteLine($"MESSAGE INSTANCE : {JsonConvert.SerializeObject(context.MessageInstance)}");
                Console.WriteLine($"HANDLER TYPE     : {context.HandlerInstance.GetType()}");
            }

            public async Task OnExecuted(DispatchingContext context)
            {
                await Task.CompletedTask;
                Console.WriteLine($"RESULT: {JsonConvert.SerializeObject(context.Result)}");
                Console.WriteLine();
            }

            public async Task OnException(DispatchingContext context, Exception invocationException)
            {
                await Task.CompletedTask;
            }
        }
    }
}