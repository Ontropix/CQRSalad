namespace Samples.Domain.Events.User
{
    public sealed class FollowerAddedEvent
    {
        public string UserId { get; set; }
        public string FollowerUserId { get; set; }
    }
}