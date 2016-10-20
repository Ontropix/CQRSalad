using CQRSalad.Domain;
using CQRSalad.EventSourcing;

namespace Samples.Domain.Interface.User
{
    public class CreateUser : ICommand
    {
        [AggregateId]
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}