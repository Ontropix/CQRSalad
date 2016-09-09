using CQRSalad.Domain;

namespace Samples.Domain.Interface.User
{
    public class UserFollowingsCountQuery : IQueryFor<int>
    {
        public string UserId { get; set; }
    }
}
