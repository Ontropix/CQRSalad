namespace Samples.Domain.Events.User.Events
{
    public sealed class FollowerAddedEvent : Event
    {
        public string UserId { get; set; }
    }
}