using System.Collections.Generic;
using Samples.Domain.User.Data;

namespace Samples.Domain.User.Queries
{
    public class UserFollowingsQuery : IQuery<List<UserSummary>>
    {
        public string UserId { get; set; }
    }
}
