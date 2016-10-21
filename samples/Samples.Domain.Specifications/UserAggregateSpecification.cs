using System;
using CQRSalad.EventSourcing.Specification;
using CQRSalad.EventSourcing.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samples.Domain.Interface.User;
using Samples.Domain.Model.User;

namespace Samples.Domain.Specifications
{
    internal static class UserScenarios
    {
        internal static UserCreated UserCreatedEvent(string userId, string email, string userName)
        {
            return new UserCreated
            {
                UserId = userId,
                Email = email
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

            Given();

            When(new CreateUser
            {
                UserId = userId,
                Email = userEmail,
            });

            Expected(new UserCreated
            {
                UserId = userId,
                Email = userEmail,
            });
        }

        //[TestMethod]
        //public void When_FollowUserCommand_ShouldBe_UserCreatedEvent()
        //{
        //    string userId_1 = Guid.NewGuid().ToString();
        //    string userEmail_1 = "john.doe@gmail.com";
        //    string userName_1 = "johndoe";

        //    string userId_2 = Guid.NewGuid().ToString();
        //    string userEmail_2 = "john.doe@gmail.com";
        //    string userName_2 = "johndoe";

        //    Given(
        //        UserScenarios.UserCreatedEvent(userId_1, userEmail_1, userName_1),
        //        UserScenarios.UserCreatedEvent(userId_2, userEmail_2, userName_2)
        //        );

        //    When(new FollowUserCommand
        //    {
        //        UserId = userId_1,
        //        FollowingUserId = userId_2
        //    });

        //    Expected(new UserFollowedEvent
        //    {
        //        UserId = userId_1,
        //        FollowingUserId = userId_2
        //    });
        //}
    }
}
