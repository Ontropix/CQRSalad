using System.Collections.Generic;
using Samples.Domain.Events.User.Events;

namespace Samples.Domain.Events.User
{
    public class UserState
    {
        public HashSet<string> Followers { get; }
        public HashSet<string> Following { get; }

        public int Test { get; set; } = 5;
        public List<int> TestList { get; set; }

        public UserState()
        {
            Followers = new HashSet<string>();
            Following = new HashSet<string>();
        }
        
        public void Apply(UserFollowedEvent evnt)
        {
            Following.Add(evnt.UserId);
        }

        public void Apply(FollowerAddedEvent evnt)
        {
            Followers.Add(evnt.UserId);
        }
    }
}