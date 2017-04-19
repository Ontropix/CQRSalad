using Samples.Domain.User;
using Samples.ViewModel.Views;

namespace Samples.ViewModel
{
    internal static class ViewExtensions
    {
        internal static UserProfile ToModel(this UserView view)
        {
            return new UserProfile
            {
                Id = view.Id,
                Email = view.Email
            };
        }
    }
}
