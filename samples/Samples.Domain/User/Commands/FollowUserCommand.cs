using CQRSalad.EventSourcing;

namespace Samples.Domain.Interface.User
{
    public sealed class FollowUserCommand
    {
        [AggregateId]
        public string UserId { get; set; }
        public string FollowingUserId { get; set; }
    }
}