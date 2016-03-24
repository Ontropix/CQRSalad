using System;
using System.Threading.Tasks;
using Kutcha.Core;
using Samples.Domain.Interface.User;
using Samples.View.Views;

namespace Samples.View.QueryHandlers
{
    public class UserQueryHandler
    {
        private readonly IKutchaReadOnlyStore<UserView> _store;

        public UserQueryHandler(IKutchaReadOnlyStore<UserView> store)
        {
            _store = store;
        }

        public async Task<UserProfile> Query(UserProfileByIdQuery query)
        {
            UserView view = await _store.FindByIdAsync(query.UserId);
            UserProfile profile = new UserProfile
            {
                Id = view.Id,
                UserName = view.UserName,
                Email = view.Email,
                FollowersCount = view.FollowersCount,
                FollowingsCount = view.FollowingsCount
            };

            return profile;
        }

        public async Task<int> Query(UserFollowingsCountQuery query)
        {
            UserView view = await _store.FindByIdAsync(query.UserId);
            return view.FollowingsCount;
        }

        public async Task<int> Query(UserFollowersCountQuery query)
        {
            UserView view = await _store.FindByIdAsync(query.UserId);
            return view.FollowersCount;
        }
    }
}
