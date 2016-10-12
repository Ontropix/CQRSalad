using System.Threading.Tasks;
using CQRSalad.Dispatching;
using CQRSalad.Dispatching.Priority;
using Kutcha.Core;
using Samples.Domain.Events.User;
using Samples.View.Views;

namespace Samples.View.ViewHandlers
{
    [DispatcherHandler]
    [DispatchingPriority(DispatchingPriority.High)]
    public sealed class UserViewHandler
    {
        private readonly IKutchaStore<UserView> _store;

        public UserViewHandler(IKutchaStore<UserView> store)
        {
            _store = store;
        }

        public async Task Apply(UserCreatedEvent evnt)
        {
            await _store.InsertAsync(new UserView
            {
                Id = evnt.UserId,
                Email = evnt.Email,
                UserName = evnt.UserName
            });
        }

        public async Task Apply(UserFollowedEvent evnt)
        {
            await _store.FindOneAndUpdateAsync(evnt.UserId, view => view.FollowingIds.Add(evnt.FollowingUserId));
        }
        
        public async Task Apply(FollowerAddedEvent evnt)
        {
            await _store.FindOneAndUpdateAsync(evnt.UserId, view => view.FollowersIds.Add(evnt.FollowerUserId));
        }
    }
}
