using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using CQRSalad.Dispatching;
using CQRSalad.Dispatching.ActionsScanning;
using CQRSalad.Dispatching.Context;
using CQRSalad.Dispatching.Core;
using CQRSalad.Dispatching.Descriptors;
using CQRSalad.Dispatching.HandlersScanning;
using CQRSalad.Dispatching.Interceptors;
using CQRSalad.Dispatching.Priority;
using CQRSalad.Dispatching.Subscriptions;
using CQRSalad.Dispatching.TypesScanning;
using CQRSalad.EventSourcing;
using Newtonsoft.Json;
using Samples.Tests.Structuremap;
using StructureMap;

namespace Samples.Tests.Configurators
{
    public static class DispatcherConfigurator
    {
        public static IContainer UseAssemblyRuleScanning(this IContainer container)
        {
            Assembly applicationServices = ApplicationServiceGenerator.Generate(typeof(Domain.Model._namespace).Assembly);

            var rules = new List<AssemblyScanningRule>
            {
                new AssemblyScanningRule(applicationServices),                                               //for application services
                new AssemblyScanningRule(typeof(Samples.Domain.Workflow._namespace).Assembly),                  //for workflow services
                new AssemblyScanningRule(typeof(Samples.View._namespace).Assembly),                  //for view handlers
                new AssemblyScanningRule(typeof(Samples.View.Querying._namespace).Assembly)                  //for query handlers
            };
            
            var typeProvider = new AssemblyTypesProvider(rules);
            container.Configure(expression => expression.For<IDispatcherTypesProvider>().Use(typeProvider).Singleton());
            
            var handlersProvider = new DefaultDispatcherHandlersProvider(typeProvider);
            container.Configure(expression => expression.For<IDispatcherHandlersProvider>().Use(handlersProvider).Singleton());

            var handlerActionsProvider = new DefaultDispatcherHandlerActionsProvider();
            container.Configure(expression => expression.For<IDispatcherHandlerActionsProvider>().Use(handlerActionsProvider).Singleton());

            var priorityProvider = new DefaultDispatcherPriorityProvider();

            var subscriptionManager = new DispatcherSubscriptionsManager(
                handlersProvider,
                new DefaultDispatcherHandlerDescriptorsBuilder(priorityProvider),
                new DefaultDispatcherHandlerActionDescriptorsBuilder(handlerActionsProvider, priorityProvider)
            );
            container.Configure(expression => expression.For<DispatcherSubscriptionsManager>().Use(subscriptionManager).Singleton());

            return container;
        }

        public static IContainer UseAsyncDispatcherSingleton(this IContainer container)
        {
            IMessageDispatcher dispatcher = Dispatcher.Create(configuration =>
            {
                configuration.SetServiceProvider(new StructureMapServiceProvider(container));

                var subscriptionManager = container.GetInstance<DispatcherSubscriptionsManager>();
                configuration.SetSubscriptionStore(subscriptionManager.Subscribe());

                configuration.AddInterceptor<ConsoleInterceptor>();
            });

            container.Configure(expression => expression.For<IMessageDispatcher>().Use(dispatcher).Singleton());
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