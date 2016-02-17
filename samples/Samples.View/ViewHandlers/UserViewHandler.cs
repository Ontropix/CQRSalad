using System.Threading.Tasks;
using Kutcha.Core;
using Platform.Domain.Extended.Handlers;
using Samples.Domain.Events.User.Events;
using Samples.View.Views;

namespace Samples.View.ViewHandlers
{
    public sealed class UserViewHandler : KutchaViewHandler<UserView>
    {
        public UserViewHandler(IKutchaContext context)
            : base(context)
        {
        }

        public async Task Apply(UserRegisteredEvent evnt)
        {
            await Source.InsertAsync(new UserView
            {
                Id = evnt.AggregateId,
                UserName = evnt.UserName,
                FullName = evnt.FullName,
                AboutYou = evnt.AboutYou
            });
        }

        public async Task Apply(UserFollowedEvent evnt)
        {
            await Source.FindOneAndUpdateAsync(evnt.AggregateId, user =>
            {
                user.FollowingIds.Add(evnt.UserId);
                user.FollowingsCount++;
            });
        }
        
        public async Task Apply(FollowerAddedEvent evnt)
        {
            await Source.FindOneAndUpdateAsync(evnt.AggregateId, user =>
            {
                user.FollowersIds.Add(evnt.UserId);
                user.FollowersCount++;
            });
        }
    }
}
