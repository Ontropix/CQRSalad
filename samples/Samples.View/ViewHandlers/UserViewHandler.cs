﻿using System.Threading.Tasks;
using CQRSalad.Dispatching;
using Kutcha.Core;
using Samples.Domain.User;
using Samples.ViewModel.Views;

namespace Samples.ViewModel.ViewHandlers
{
    [DispatcherHandler]
    [DispatchingPriority(Priority.High)]
    public sealed class UserViewHandler
    {
        private readonly IKutchaStore<UserView> _store;

        public UserViewHandler(IKutchaStore<UserView> store)
        {
            _store = store;
        }

        public async Task Apply(UserCreated evnt)
        {
            await _store.InsertAsync(new UserView
            {
                Id = evnt.UserId,
                Email = evnt.Email,
            });
        }
    }
}
