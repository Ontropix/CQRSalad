using System;
using System.Reflection;
using System.Threading.Tasks;
using CQRSalad.Dispatching;
using CQRSalad.Infrastructure;
using CQRSalad.Infrastructure.CodeGeneration;
using CQRSalad.Infrastructure.Interceptors;
using Newtonsoft.Json;
using Samples.Domain.Interface.User;
using Samples.Tests.Structuremap;
using Samples.ViewModel.Views;
using StructureMap;

namespace Samples.Tests.Configurators
{
    public static class DispatcherExtensions
    {
        public static Dispatcher Create(Action<DispatcherConfig> configurator)
        {
            var config = new DispatcherConfig();
            configurator(config);
            Dispatcher instance = Dispatcher.Create(config);
            return instance;
        }
    }

    public static class DispatcherConfigurator
    {
        public static IContainer UseDispatcher(this IContainer container)
        {
            Assembly applicationServices = ApplicationServiceGenerator.Generate(typeof(UserAggregate).Assembly);

            Dispatcher dispatcher = DispatcherExtensions.Create(configuration =>
            {
                configuration.RegisterHandlers(applicationServices);
                configuration.RegisterHandlers(typeof(WorkflowService).Assembly);
                configuration.RegisterHandlers(typeof(IView).Assembly);

                configuration.SetServiceProvider(new StructureMapServiceProvider(container));
                configuration.AddInterceptor<ConsoleInterceptor>();
                configuration.ThrowIfMultipleSendingHandlersFound = true;
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