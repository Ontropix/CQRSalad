using CQRSalad.Domain;

namespace Samples.Domain.Interface.User
{
    public class UserProfileByIdQuery : IQueryFor<UserProfile>
    {
        public string UserId { get; set; }
    }
}
