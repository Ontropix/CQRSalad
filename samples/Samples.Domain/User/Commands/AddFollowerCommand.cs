using Platform.EventSourcing;

namespace Samples.Domain.User.Commands
{
    public sealed class AddFollowerCommand
    {
        [AggregateId]
        public string TargetUserId { get; set; }
        public string UserId { get; set; }
    }
}