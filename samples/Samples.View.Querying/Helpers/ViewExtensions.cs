using Samples.Domain.Model.User;
using Samples.View.Views;

namespace Samples.View.Querying
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
