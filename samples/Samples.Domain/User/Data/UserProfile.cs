namespace Samples.Domain.Interface.User
{
    public class UserProfile
    {
        public string Id { get; set; }
        public string UserName { get; set; }

        public int FollowersCount { get; set; }
        public int FollowingsCount { get; set; }
    }
}
