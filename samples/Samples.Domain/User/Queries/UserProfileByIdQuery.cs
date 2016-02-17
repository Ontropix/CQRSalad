using Samples.Domain.User.Data;

namespace Samples.Domain.Interface.User
{
    public class UserProfileByIdQuery : IQuery<UserProfile>
    {
        public string UserId { get; set; }
    }
}
