using NUnit.Framework;
using Platform.Domain;
using Samples.Domain.Interface.User;
using Samples.Domain.User.Commands;
using Samples.Domain.User.Data;
using Samples.Domain.User.Queries;
using StructureMap;

namespace Samples.Startup
{
    [TestFixture]
    public class UserTests
    {
        private IContainer _container;

        private const string userId = "1";
        private const string sender = "UNITTEST";

        [Test]
        public async void TestMethod1()
        {
            _container = new Container();
            //    _container.UseGuidIdGenerator();
            //    _container.UseAsyncDispatcherSingleton();
            //    _container.UseInMemoryBuses();
            //    //_container.UseMongoEventStore("mongodb://admin:1@localhost/test_domain");
            //    _container.UseInMemoryEventStore();
            //    _container.UseCommandProcessorSingleton();
            //    _container.UseInMemoryKutcha();

            var commandBus = _container.GetInstance<ICommandBus>();

            await commandBus.SendAsync(new RegisterUserCommand
            {
                UserName = "Tolya",
                FullName = "Orl",
                AboutYou = "Police"
            }, sender);

            for (int i = 0; i < 5; i++)
            {
                    await commandBus.SendAsync(new FollowUserCommand
                    {
                        UserId = $"{i}"
                    }, sender);

            }

            var queryBus = _container.GetInstance<IQueryBus>();
            QueryResult<UserProfile> user = await queryBus.QueryAsync(new UserProfileByIdQuery() { UserId = userId }, sender);
        }
    }
}
