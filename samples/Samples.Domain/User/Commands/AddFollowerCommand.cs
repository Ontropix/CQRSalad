namespace Samples.Domain.Interface.User
{
    public sealed class AddFollowerCommand
    {
        [AggregateId]
        public string TargetUserId { get; set; }
        public string UserId { get; set; }
    }
}