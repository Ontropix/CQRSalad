using CQRSalad.Infrastructure;

namespace Samples.Domain.Model.User
{
    public class UserProfileById : IQueryFor<UserProfile>
    {
        public string UserId { get; set; }
    }
}
