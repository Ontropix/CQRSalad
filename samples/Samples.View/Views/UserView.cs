using System.Collections.Generic;

namespace Samples.View.Views
{
    public sealed class UserView : IView
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public HashSet<string> FollowersIds { get; set; } = new HashSet<string>();
        public HashSet<string> FollowingIds { get; set; } = new HashSet<string>();
    }
}
