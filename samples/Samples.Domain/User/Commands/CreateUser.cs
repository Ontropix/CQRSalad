using CQRSalad.EventSourcing;

namespace Samples.Domain.User
{
    public class CreateUser : ICommand
    {
        [AggregateId]
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}