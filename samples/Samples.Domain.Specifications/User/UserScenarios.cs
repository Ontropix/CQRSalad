using Samples.Domain.Model.User;

namespace Samples.Domain.Specifications
{
    internal static class UserScenarios
    {
        internal static UserCreated UserCreatedEvent(string userId, string email, string userName)
        {
            return new UserCreated
            {
                UserId = userId,
                Email = email
            };
        }
    }
}