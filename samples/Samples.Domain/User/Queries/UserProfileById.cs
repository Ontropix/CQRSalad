using CQRSalad.Domain;

namespace Samples.Domain.Interface.User
{
    public class UserProfileById : IQueryFor<UserProfile>
    {
        public string UserId { get; set; }
    }
}
