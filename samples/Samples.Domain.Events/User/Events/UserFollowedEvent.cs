namespace Samples.Domain.Events.User
{
    public sealed class UserFollowedEvent
    {
        public string UserId { get; set; }
        public string FollowingUserId { get; set; }
    }
}