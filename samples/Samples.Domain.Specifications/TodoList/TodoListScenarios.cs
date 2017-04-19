using Samples.Domain.User;

namespace Samples.Domain.Specifications
{
    internal static class TodoListScenarios
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