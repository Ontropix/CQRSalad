using CQRSalad.Domain;

namespace Samples.Domain.Interface.User
{
    public class UserFollowersCountQuery : IQuery<int>
    {
        public string UserId { get; set; }
    }
}
