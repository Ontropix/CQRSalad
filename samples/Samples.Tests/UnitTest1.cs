using System;
using System.Threading.Tasks;
using CQRSalad.Domain;
using CQRSalad.Infrastructure.Buses;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samples.Domain.Interface.TodoList.Commands;
using Samples.Domain.Interface.User;
using Samples.Tests.Configurators;
using StructureMap;

namespace Samples.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private readonly IContainer container;

        public UnitTest1()
        {
            container = new Container()
                .UseGuidIdGenerator()
                .UseInMemoryKutcha()
                .RegisterKutchaRoots()
                .UseAssemblyRuleScanning()
                .UseAsyncDispatcherSingleton()
                .UseInMemoryBuses()
                .UseInMemoryEventStore()
                .UseCommandProcessorSingleton()
                .UseFluentMessageValidator();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            string userId = "1";

            var bus = container.GetInstance<IDomainBus>();

            await bus.CommandAsync(new CreateUser
            {
                UserId = userId,
                Email = "first@gmail.com",
            }, "test");

            await bus.CommandAsync(new CreateUser
            {
                UserId = "2",
                Email = "second@gmail.com",
            }, "test");

            var result = await bus.QueryAsync(new UserProfileById
            {
                UserId = userId
            }, "test");

            string listId = "list1";
            await bus.CommandAsync(new CreateTodoList
            {
                ListId = listId,
                OwnerId = userId,
                Title = "Buy in the shop"
            }, "test");

            await bus.CommandAsync(new AddListItem
            {
                ListId = listId,
                ItemId = "1",
                Description = "Milk"
            }, "test");

            await bus.CommandAsync(new AddListItem
            {
                ListId = listId,
                ItemId = "2",
                Description = "Meat"
            }, "test");

            await bus.CommandAsync(new RemoveListItem
            {
                ListId = listId,
                ItemId = "2"
            }, "test");

            await bus.CommandAsync(new AddListItem
            {
                ListId = listId,
                ItemId = "3",
                Description = "Bread"
            }, "test");

            await bus.CommandAsync(new CompleteListItem
            {
                ListId = listId,
                ItemId = "3"
            }, "test");
        }
    }
}