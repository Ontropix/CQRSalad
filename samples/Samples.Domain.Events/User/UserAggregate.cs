using System.Collections;
using System.Collections.Generic;
using Platform.EventSourcing;
using Platform.EventStore.Core;
using Samples.Domain.Events.User.Events;
using Samples.Domain.Interface.User;
using Samples.Domain.User.Commands;

namespace Samples.Domain.Events.User
{
    public class UserAggregate : AggregateRoot<UserState>
    {
        [AggregateCtor]
        public void When(RegisterUserCommand command)
        {
            ProduceEvent(command.MapTo<UserRegisteredEvent>());
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