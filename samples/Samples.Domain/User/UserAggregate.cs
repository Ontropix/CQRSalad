using CQRSalad.EventSourcing;

namespace Samples.Domain.User
{
    public class UserAggregate : AggregateRoot<UserState>
    {
        [AggregateCtor]
        public void When(CreateUser command)
        {
            ProduceEvent(command.MapToEvent<UserCreated>());
        }

        public void When(RemoveUser command)
        {
            ProduceEvent(command.MapToEvent<UserRemoved>());
        }
    }
}