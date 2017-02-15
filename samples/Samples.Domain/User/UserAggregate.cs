using CQRSalad.EventSourcing;
using Samples.Domain.Model.User;

namespace Samples.Domain.Interface.User
{
    public class UserAggregate : AggregateRoot<UserState>
    {
        [AggregateCtor]
        public void When(CreateUser command)
        {
            ProduceEvent(command.MapToEvent<UserCreated>());
        }
    }
}