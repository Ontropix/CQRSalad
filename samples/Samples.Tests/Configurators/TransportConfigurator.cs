﻿using System;
using System.Reflection;
using CQRSalad.Infrastructure;
using CQRSalad.Infrastructure.Validation;
using Kutcha.Core;
using Kutcha.InMemory;
using Samples.Domain.TodoList;
using Samples.Tests.Structuremap;
using Samples.ViewModel.SingleUseHandlers;
using Samples.ViewModel.Views;
using StructureMap;

namespace Samples.Tests.Configurators
{
    public static class TransportConfigurator
    {
        public static IContainer UseInMemoryBuses(this IContainer container)
        {
            container.Configure(expression => expression.For<ICommandBus>().Use<InMemoryCommandBus>().Singleton());
            container.Configure(expression => expression.For<IQueryBus>().Use<InMemoryQueryBus>().Singleton());
            container.Configure(expression => expression.For<IDomainBus>().Use<InMemoryDomainBus>().Singleton());
            return container;
        }

        public static IContainer UseGuidIdGenerator(this IContainer container)
        {
            IIdGenerator idGenerator = new GuidIdGenerator();
            container.Configure(expression => expression.For<IIdGenerator>().Use(idGenerator));

            container.Configure(expression => expression.For<IEmailSender>().Use<MockEmailSender>());

            return container;
        }

        public static IContainer UseFluentMessageValidator(this IContainer container)
        {
            var controller = new ValidationController(new StructureMapServiceProvider(container));
            controller.RegisterFromAssemblies(typeof(CreateTodoListValidator).Assembly);
            container.Configure(expression => expression.For<ValidationController>().Use(controller).Singleton());
            return container;
        }

        public static IContainer UseInMemoryKutcha(this IContainer container)
        {
            container.Configure(config => config.For<IKutchaContext>().Use<InMemoryKutchaContext>().Singleton());
            return container;
        }

        public static IContainer RegisterKutchaRoots(this IContainer container)
        {
            container.Configure(config =>
                                    config.Scan(scan =>
                                    {
                                        scan.Assembly(typeof(IView).Assembly);
                                        scan.WithDefaultConventions();
                                        scan.AddAllTypesOf(typeof(IKutchaRoot));
                                    })
                );

            IKutchaContext context = container.GetInstance<IKutchaContext>();
            foreach (IKutchaRoot root in container.GetAllInstances<IKutchaRoot>())
            {
                Type rootType = root.GetType();

                Type storeType = typeof(IKutchaStore<>).MakeGenericType(rootType);
                MethodInfo method = context.GetType().GetMethod("GetStore").MakeGenericMethod(rootType);
                container.Configure(config => config.For(storeType).Use(method.Invoke(context, null)));

                Type readStoreType = typeof(IKutchaReadOnlyStore<>).MakeGenericType(rootType);
                MethodInfo methodRead = context.GetType().GetMethod("GetReadOnlyStore").MakeGenericMethod(rootType);
                container.Configure(config => config.For(readStoreType).Use(methodRead.Invoke(context, null)));
            }
            
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