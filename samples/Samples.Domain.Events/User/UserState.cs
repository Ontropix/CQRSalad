using System.Collections.Generic;

namespace Samples.Domain.Events.User
{
    public class UserState
    {
        public HashSet<string> Followers { get; } = new HashSet<string>();
        public HashSet<string> Following { get; } = new HashSet<string>();
        
        public void Apply(UserFollowedEvent evnt)
        {
            Following.Add(evnt.FollowingUserId);
        }

        public void Apply(FollowerAddedEvent evnt)
        {
            Followers.Add(evnt.FollowerUserId);
        }
    }
}