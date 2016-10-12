using System;
using System.Threading.Tasks;
using CQRSalad.Domain;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samples.Domain.Interface.User;
using Samples.Tests.Configurators;
using StructureMap;

namespace Samples.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private Container container;

        public UnitTest1()
        {
            container = new Container();
            container.UseGuidIdGenerator();
            container.UseInMemoryKutcha();
            container.RegisterKutchaRoots();
            container.UseAssemblyRuleScanning();
            container.UseAsyncDispatcherSingleton();
            container.UseInMemoryBuses();
            container.UseInMemoryEventStore();
            container.UseCommandProcessorSingleton();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            string userId = "1";

            var commandBus = container.GetInstance<ICommandBus>();
            await commandBus.SendAsync(new CreateUserCommand()
            {
                UserId = userId,
                Email = "first@gmail.com",
                UserName = "first"
            }, "test");

            await commandBus.SendAsync(new CreateUserCommand()
            {
                UserId = "2",
                Email = "second@gmail.com",
                UserName = "second"
            }, "test");

            var queryBus = container.GetInstance<IQueryBus>();
            var result = await queryBus.QueryAsync(new UserProfileByIdQuery()
            {
                UserId = userId
            }, "test");
            
            Console.WriteLine(result);

            await commandBus.SendAsync(new FollowUserCommand
            {
                UserId = userId,
                FollowingUserId = "2"
            }, "test");
        }
    }
}