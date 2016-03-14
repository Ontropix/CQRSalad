using CQRSalad.Domain;

namespace Samples.Domain.Interface.User
{
    public class UserFollowingsCountQuery : IQuery<int>
    {
        public string UserId { get; set; }
    }
}
