using System.Collections.Generic;
using CQRSalad.EventSourcing;
using CQRSalad.EventSourcing.Specifications;
using Samples.Domain.User;

namespace Samples.Domain.Specifications
{
    public class RemoveUserSpecification : AggregateSpecification<UserAggregate>
    {
        private readonly string _userId;

        public RemoveUserSpecification(string userId)
        {
            _userId = userId;
        }

        public override IEnumerable<IEvent> Given()
        {
            yield return UserScenarios.UserCreatedEvent(_userId, "testuser@mail.com");
        }

        public override ICommand When()
        {
            return new RemoveUser
            {
                UserId = _userId
            };
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new UserRemoved
            {
                UserId = _userId
            };
        }
    }
}