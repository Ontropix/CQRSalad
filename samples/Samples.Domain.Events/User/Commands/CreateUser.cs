using CQRSalad.EventSourcing;

namespace Samples.Domain.Model.User
{
    public class CreateUser : ICommand
    {
        [AggregateId]
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}