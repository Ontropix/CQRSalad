using Samples.Domain.User;

namespace Samples.Domain.Specifications
{
    internal static class UserScenarios
    {
        internal static UserCreated UserCreatedEvent(string userId, string email)
        {
            return new UserCreated
            {
                UserId = userId,
                Email = email
            };
        }
    }
}