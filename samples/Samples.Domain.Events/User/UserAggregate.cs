using CQRSalad.EventSourcing;
using Samples.Domain.Interface.User;

namespace Samples.Domain.Events.User
{
    public class UserAggregate : AggregateRoot<UserState>
    {
        [AggregateCtor]
        public void When(CreateUserCommand command)
        {
        }
        
        public void When(FollowUserCommand command)
        {
            //ProduceError("User is already in following list.", state => state.Following.Contains(command.UserId));
            //ProduceEvent(command.MapTo<UserFollowedEvent>());
        }

        public void When(AddFollowerCommand command)
        {
           // ProduceError("User is already in followers list.", state => state.Followers.Contains(command.UserId));
            //ProduceEvent(command.MapTo<FollowerAddedEvent>());
        }
    }
}