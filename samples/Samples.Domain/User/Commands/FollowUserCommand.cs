using CQRSalad.Domain;
using CQRSalad.EventSourcing;

namespace Samples.Domain.Interface.User
{
    public sealed class FollowUserCommand : ICommand
    {
        [AggregateId]
        public string UserId { get; set; }
        public string FollowingUserId { get; set; }
    }
}