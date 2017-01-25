using System.Threading.Tasks;
using CQRSalad.Dispatching;
using Kutcha.Core;
using Samples.Domain.Interface.User;
using Samples.View.Views;

namespace Samples.View.Querying.QueryHandlers
{
    [DispatcherHandler]
    public class UserQueryHandler
    {
        private readonly IKutchaReadOnlyStore<UserView> _store;

        public UserQueryHandler(IKutchaReadOnlyStore<UserView> store)
        {
            _store = store;
        }

        public async Task<UserProfile> Query(UserProfileById query)
        {
            UserView view = await _store.FindByIdAsync(query.UserId);
            return view?.ToModel();
        }
    }
}
