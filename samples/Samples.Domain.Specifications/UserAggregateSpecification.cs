using System;
using CQRSalad.EventSourcing.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samples.Domain.Events.User;
using Samples.Domain.Interface.User;

namespace Samples.Domain.Specifications
{
    internal static class Emitter
    {
        internal static UserCreatedEvent UserCreatedEvent(string userId, string email, string userName)
        {
            return new UserCreatedEvent
            {
                UserId = userId,
                Email = email,
                UserName = userName
            };
        }
    }

    [TestClass]
    public class UserAggregateSpecification : AggregateSpecification<UserAggregate>
    {
        [TestMethod]
        public void When_CreateUserCommand_ShouldBe_UserCreatedEvent()
        {
            string userId = Guid.NewGuid().ToString();
            string userEmail = "john.doe@gmail.com";
            string userName = "johndoe";

            Given();

            When(new CreateUserCommand
            {
                UserId = userId,
                Email = userEmail,
                UserName = userName
            });

            Expected(new UserCreatedEvent
            {
                UserId = userId,
                Email = userEmail,
                UserName = userName
            });
        }

        [TestMethod]
        public void When_FollowUserCommand_ShouldBe_UserCreatedEvent()
        {
            string userId_1 = Guid.NewGuid().ToString();
            string userEmail_1 = "john.doe@gmail.com";
            string userName_1 = "johndoe";

            string userId_2 = Guid.NewGuid().ToString();
            string userEmail_2 = "john.doe@gmail.com";
            string userName_2 = "johndoe";

            Given(
                Emitter.UserCreatedEvent(userId_1, userEmail_1, userName_1),
                Emitter.UserCreatedEvent(userId_2, userEmail_2, userName_2)
                );

            When(new FollowUserCommand
            {
                UserId = userId_1,
                FollowingUserId = userId_2
            });

            Expected(new UserFollowedEvent
            {
                UserId = userId_1,
                FollowingUserId = userId_2
            });
        }
    }
}
