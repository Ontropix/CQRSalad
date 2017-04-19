using CQRSalad.EventSourcing;

namespace Samples.Domain.User
{
    public class RemoveUser : ICommand
    {
        [AggregateId]
        public string UserId { get; set; }
    }
}