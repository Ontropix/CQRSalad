using Samples.Domain.User.Data;

namespace Samples.Domain.Events.User.Events
{
    public class UserRegisteredEvent : Event
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AboutYou { get; set; }
    }
}