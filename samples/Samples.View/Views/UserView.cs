using System.Collections.Generic;
using Samples.Domain.User.Data;

namespace Samples.View.Views
{
    public sealed class UserView : IView
    {
        public UserView()
        {
            FollowersIds = new HashSet<string>();
            FollowingIds = new HashSet<string>();
            
            StoryIds = new List<string>();
            PostIds = new List<string>();
        }

        public string Id { get; set; }

        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AboutYou { get; set; }

        public HashSet<string> FollowersIds { get; set; }
        public int FollowersCount { get; set; }

        public HashSet<string> FollowingIds { get; set; }
        public int FollowingsCount { get; set; }

        public List<string> StoryIds { get; set; }
        public List<string> PostIds { get; set; }
    }
}
