using Platform.Domain;
using Samples.Domain.User.Data;

namespace Samples.Domain.User.Queries
{
    public class UserProfileByIdQuery : IQuery<UserProfile>
    {
        public string UserId { get; set; }
    }
}
