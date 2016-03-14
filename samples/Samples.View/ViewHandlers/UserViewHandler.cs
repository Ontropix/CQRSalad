using System;
using System.Threading.Tasks;
using Samples.Domain.Events.User.Events;
using Samples.View.Views;

namespace Samples.View.ViewHandlers
{
    public sealed class UserViewHandler
    {
        public async Task Apply(UserRegisteredEvent evnt)
        {
            throw new NotImplementedException();
        }

        public async Task Apply(UserFollowedEvent evnt)
        {
            throw new NotImplementedException();
        }
        
        public async Task Apply(FollowerAddedEvent evnt)
        {
            throw new NotImplementedException();
        }
    }
}
