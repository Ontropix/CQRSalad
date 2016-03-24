using System.Collections.Generic;

namespace Samples.View.Views
{
    public sealed class UserView : IView
    {
        public UserView()
        {
            FollowersIds = new HashSet<string>();
            FollowingIds = new HashSet<string>();
        }

        public string Id { get; set; }
        
        public string UserName { get; set; }
        public string Email { get; set; }

        public HashSet<string> FollowersIds { get; set; }
        public int FollowersCount => FollowersIds.Count;

        public HashSet<string> FollowingIds { get; set; }
        public int FollowingsCount => FollowingIds.Count;
    }
}
