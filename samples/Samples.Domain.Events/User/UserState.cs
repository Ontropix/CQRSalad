using System.Collections.Generic;

namespace Samples.Domain.Events.User
{
    public class UserState
    {
        public HashSet<string> Followers { get; }
        public HashSet<string> Following { get; }
        
        public UserState()
        {
            Followers = new HashSet<string>();
            Following = new HashSet<string>();
        }
        
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