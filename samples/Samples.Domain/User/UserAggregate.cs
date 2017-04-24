using CQRSalad.EventSourcing;

namespace Samples.Domain.User
{
    public class UserAggregate : AggregateRoot<UserState>
    {
        [AggregateConstructor]
        public void When(CreateUser command)
        {
            ProduceEvent(command.MapToEvent<UserCreated>());
        }

        [AggregateDestructor]
        public void When(RemoveUser command)
        {
            ProduceEvent(command.MapToEvent<UserRemoved>());
        }
    }
}