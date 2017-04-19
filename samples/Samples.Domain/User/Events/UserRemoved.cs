using CQRSalad.EventSourcing;

namespace Samples.Domain.User
{
    public class UserRemoved : IEvent
    {
        public string UserId { get; set; }
    }
}