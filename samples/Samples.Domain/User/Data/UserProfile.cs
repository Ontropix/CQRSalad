using System.Collections.Generic;

namespace Samples.Domain.User.Data
{
    public class UserProfile
    {
        public string Id { get; set; }

        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AboutYou { get; set; }
        public string AvatarUri { get; set; }

        public HashSet<string> FollowersIds { get; set; }
        public int FollowersCount { get; set; }

        public HashSet<string> FollowingIds { get; set; }
        public int FollowingsCount { get; set; }
    }
}
