using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Kutcha.Core;
using Kutcha.Core.Extensions;
using Platform.Domain.Extended.Handlers;
using Samples.Domain.User.Data;
using Samples.Domain.User.Queries;
using Samples.View.Views;

namespace Samples.View.QueryHandlers
{
    public class UserQueryHandler : KutchaQueryHandler<UserView>
    {
        public UserQueryHandler(IReadOnlyKutchaContext context) : base(context)
        {
        }

        public async Task<List<UserSummary>> Query(UserFollowingsQuery query)
        {
            UserView userView = await Source.FindByIdAsync(query.UserId);
            return await GetUserSummaries(userView.FollowingIds.ToList());
        }

        public async Task<List<UserSummary>> Query(UserFollowersQuery query)
        {
            UserView userView = await Source.FindByIdAsync(query.UserId);
            return await GetUserSummaries(userView.FollowersIds.ToList());
        }

        public async Task<UserProfile> Query(UserProfileByIdQuery query)
        {
            UserView userView = await Source.FindByIdAsync(query.UserId);
            return userView.MaybeReturn(new UserProfile()
            {
                Id = userView.Id,
                FullName = userView.FullName,
                UserName = userView.UserName,
                AboutYou = userView.AboutYou,

                FollowersIds = userView.FollowersIds,
                FollowersCount = userView.FollowersCount,

                FollowingIds = userView.FollowingIds,
                FollowingsCount = userView.FollowingsCount
            });
        }

        private async Task<List<UserSummary>> GetUserSummaries(List<string> ids)
        {
            if (ids == null || ids.Count == 0) 
            {
                return new List<UserSummary>();
            }

            List<UserView> views = await Source.FindByIdsAsync(ids.ToArray());

            var users = new ConcurrentBag<UserSummary>();
            Parallel.ForEach(views, x => users.Add(ToSummary(x)));
            return users.ToList();
        }

        private static UserSummary ToSummary(UserView view)
        {
            return view.MaybeReturn(new UserSummary
            {
                Id = view.Id,
                UserName = view.UserName
            });
        }
    }
}
