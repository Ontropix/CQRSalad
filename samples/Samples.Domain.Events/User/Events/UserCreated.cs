using CQRSalad.EventStore.Core;

namespace Samples.Domain.Model.User
{
    public class UserCreated : IEvent
    {
        public string UserId { get; set; }
        public string Email { get; set; }
    }
}