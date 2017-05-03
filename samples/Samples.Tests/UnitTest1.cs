using System;
using System.Threading.Tasks;
using CQRSalad.EventSourcing;
using CQRSalad.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Samples.Domain.TodoList;
using Samples.Domain.User;
using Samples.Tests.Configurators;
using Samples.Tests.EventStore;
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
                .UseDispatcher()
                .UseInMemoryBuses()
                .UseInMemoryEventStore()
                .UseCommandProcessorSingleton()
                .UseFluentMessageValidator();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            string user1Id = Guid.NewGuid().ToString();
            string todoList1Id = Guid.NewGuid().ToString();
            
            var eventStore = container.GetInstance<IEventStoreAdapter>();

            var bus = container.GetInstance<IDomainBus>();
            await bus.CommandAsync(new CreateUser
            {
                UserId = user1Id,
                Email = "first@gmail.com",
            }, "test");

            var result = await bus.QueryAsync(new UserProfileById
            {
                UserId = user1Id
            }, "test");

            
            await bus.CommandAsync(new CreateTodoList
            {
                ListId = todoList1Id,
                OwnerId = user1Id,
                Title = "Supermarket"
            }, "test");

            await bus.CommandAsync(new AddListItem
            {
                ListId = todoList1Id,
                ItemId = "1",
                Description = "Milk"
            }, "test");

            await bus.CommandAsync(new AddListItem
            {
                ListId = todoList1Id,
                ItemId = "2",
                Description = "Meat"
            }, "test");

            await bus.CommandAsync(new RemoveListItem
            {
                ListId = todoList1Id,
                ItemId = "2"
            }, "test");

            await bus.CommandAsync(new AddListItem
            {
                ListId = todoList1Id,
                ItemId = "3",
                Description = "Bread"
            }, "test");

            await bus.CommandAsync(new CompleteListItem
            {
                ListId = todoList1Id,
                ItemId = "3"
            }, "test");

            await eventStore.DeleteStreamAsync(user1Id);
            await eventStore.DeleteStreamAsync(todoList1Id);
        }
    }
}