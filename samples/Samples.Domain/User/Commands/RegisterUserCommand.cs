namespace Samples.Domain.Interface.User
{
    public class RegisterUserCommand
    {
        [AggregateId]
        public string UserId { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string AboutYou { get; set; }
    }
}