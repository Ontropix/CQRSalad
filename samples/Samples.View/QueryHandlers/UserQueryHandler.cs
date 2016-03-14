using System;
using System.Threading.Tasks;
using Samples.Domain.Interface.User;

namespace Samples.View.QueryHandlers
{
    public class UserQueryHandler
    {
        public async Task<UserProfile> Query(UserProfileByIdQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Query(UserFollowingsCountQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task<int> Query(UserFollowersCountQuery query)
        {
            throw new NotImplementedException();
        }
    }
}
