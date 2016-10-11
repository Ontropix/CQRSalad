using System;
using CQRSalad.EventSourcing.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samples.Domain.Events.User;
using Samples.Domain.Interface.User;

namespace Samples.Domain.Specifications
{
    [TestClass]
    public class UserAggregateSpecification : AggregateSpecification<UserAggregate>
    {
        [TestMethod]
        public void When_CreateUserCommand_ShouldBe_UserCreatedEvent()
        {
            string userId = Guid.NewGuid().ToString();
            string userEmail = "ivan.ivanov@gmail.com";
            string userName = "ivanivanov";

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
                UserName = userName + 1
            });
        }
    }
}
