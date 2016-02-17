using System.Collections.Generic;
using Samples.Domain.User.Data;

namespace Samples.Domain.Interface.User
{
    public class UserFollowersQuery : IQuery<List<UserSummary>>
    {
        public string UserId { get; set; }
    }
}
