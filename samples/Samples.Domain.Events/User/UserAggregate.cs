using CQRSalad.EventSourcing;
using Samples.Domain.Interface.User;

namespace Samples.Domain.Model.User
{
    public class UserAggregate : AggregateRoot<UserState>
    {
        [AggregateCtor]
        public void When(CreateUserCommand command)
        {
            ProduceEvent(command.MapTo<UserCreated>());
        }
    }
}