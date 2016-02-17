namespace Samples.Domain.Events.User.Events
{
    public sealed class UserFollowedEvent : Event
    {
        public string UserId { get; set; }
    }
}