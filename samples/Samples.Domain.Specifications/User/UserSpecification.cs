using System.Collections.Generic;
using CQRSalad.EventSourcing;
using CQRSalad.Specifications;
using Samples.Domain.Interface.User;
using Samples.Domain.Model.User;

namespace Samples.Domain.Specifications
{
    public class CreateUserSpecification : AggregateSpecification<UserAggregate>
    {
        private readonly string _userId;
        private readonly string _email;

        public CreateUserSpecification(string userId, string email)
        {
            _userId = userId;
            _email = email;
        }

        public override ICommand When()
        {
            return new CreateUser
            {
                UserId = _userId,
                Email = _email
            };
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new UserCreated
            {
                UserId = _userId,
                Email = _email
            };
        }
    }
}
