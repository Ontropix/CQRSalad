using CQRSalad.Infrastructure;

namespace Samples.Domain.User
{
    public class UserProfileById : IQueryFor<UserProfile>
    {
        public string UserId { get; set; }
    }
}
