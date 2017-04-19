using System;
using System.Reflection;
using CQRSalad.Dispatching;
using CQRSalad.EventSourcing;
using CQRSalad.EventSourcing.CodeGeneration;
using Samples.Domain.User;
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

            Dispatcher dispatcher = DispatcherExtensions.Create(config =>
            {
                config.HandlersTypesResolver = type =>
                {
                    bool isDispatcherHandler = type.IsDefined(typeof(DispatcherHandlerAttribute));
                    bool isApplicationService = type.BaseType != null && type.BaseType.IsGenericType && typeof(ApplicationService<>).IsAssignableFrom(type.BaseType?.GetGenericTypeDefinition());
                    return isDispatcherHandler || isApplicationService;
                };

                config.RegisterHandlers(applicationServices);
                //config.RegisterHandlers(typeof(WorkflowService).Assembly);
                config.RegisterHandlers(typeof(IView).Assembly);

                config.SetServiceProvider(new StructureMapServiceProvider(container));
                config.AddInterceptor<ConsoleInterceptor>();
                config.ThrowIfMultipleSendingHandlersFound = true;
            });

            container.Configure(expression => expression.For<Dispatcher>().Use(dispatcher).Singleton());

            return container;
        }
    }
}