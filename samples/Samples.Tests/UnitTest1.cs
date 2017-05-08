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
            
            var eventStore = container.GetInstance<IEventStoreAdapter>();

            var bus = container.GetInstance<IDomainBus>();
            await bus.CommandAsync(new CreateUser
            {
                UserId = user1Id,
                Email = $"user_{user1Id}@gmail.com",
            }, "test");

            var result = await bus.QueryAsync(new UserProfileById
            {
                UserId = user1Id
            }, "test");


            string todoList1Id = Guid.NewGuid().ToString();
            await bus.CommandAsync(new CreateTodoList
            {
                ListId = todoList1Id,
                OwnerId = user1Id,
                Title = "Supermarket"
            }, "test");


            string todoListItem1Id = Guid.NewGuid().ToString();
            await bus.CommandAsync(new AddListItem
            {
                ListId = todoList1Id,
                ItemId = todoListItem1Id,
                Description = "Milk"
            }, "test");


            string todoListItem2Id = Guid.NewGuid().ToString();
            await bus.CommandAsync(new AddListItem
            {
                ListId = todoList1Id,
                ItemId = todoListItem2Id,
                Description = "Meat"
            }, "test");

            await bus.CommandAsync(new RemoveListItem
            {
                ListId = todoList1Id,
                ItemId = todoListItem2Id
            }, "test");

            string todoListItem3Id = Guid.NewGuid().ToString();
            await bus.CommandAsync(new AddListItem
            {
                ListId = todoList1Id,
                ItemId = todoListItem3Id,
                Description = "Bread"
            }, "test");

            await bus.CommandAsync(new CompleteListItem
            {
                ListId = todoList1Id,
                ItemId = todoListItem3Id
            }, "test");

            await bus.CommandAsync(new DeleteTodoList
            {
                ListId = todoList1Id
            }, "test");
        }
    }
}