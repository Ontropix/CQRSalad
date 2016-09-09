using CQRSalad.Domain;

namespace Samples.Domain.Interface.User
{
    public class UserFollowersCountQuery : IQueryFor<int>
    {
        public string UserId { get; set; }
    }
}
