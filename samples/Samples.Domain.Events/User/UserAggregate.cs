using CQRSalad.EventSourcing;
using Samples.Domain.Interface.User;

namespace Samples.Domain.Events.User
{
    public class UserAggregate : AggregateRoot<UserState>
    {
        [AggregateCtor]
        public void When(CreateUserCommand command)
        {
            ProduceEvent(command.MapTo<UserCreatedEvent>());
        }
        
        public void When(FollowUserCommand command)
        {
            if (State.Following.Contains(command.FollowingUserId))
            {
                ProduceError("User is already followed.");
            }

            ProduceEvent(command.MapTo<UserFollowedEvent>());
        }

        public void When(AddFollowerCommand command)
        {
            if (State.Followers.Contains(command.FollowerUserId))
            {
                ProduceError("User is already in followers list.");
            }

            ProduceEvent(command.MapTo<FollowerAddedEvent>());
        }
    }
}