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
        private readonly Container container;

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
            container.UseFluentMessageValidator();
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            string userId = "1";

            var bus = container.GetInstance<IDomainBus>();

            await bus.SendAsync(new CreateUserCommand()
            {
                UserId = userId,
                Email = "first@gmail.com",
                UserName = "first"
            }, "test");

            await bus.SendAsync(new CreateUserCommand()
            {
                UserId = "2",
                Email = "second@gmail.com",
                UserName = "second"
            }, "test");

            var result = await bus.QueryAsync(new UserProfileByIdQuery()
            {
                UserId = userId
            }, "test");
            

            await bus.SendAsync(new FollowUserCommand
            {
                UserId = userId,
                FollowingUserId = "2"
            }, "test");
        }
    }
}