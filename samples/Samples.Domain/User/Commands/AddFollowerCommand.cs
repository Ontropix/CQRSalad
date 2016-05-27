using CQRSalad.Domain;
using CQRSalad.EventSourcing;

namespace Samples.Domain.Interface.User
{
    public sealed class AddFollowerCommand : ICommand
    {
        [AggregateId]
        public string UserId { get; set; }
        public string FollowerUserId { get; set; }
    }
}