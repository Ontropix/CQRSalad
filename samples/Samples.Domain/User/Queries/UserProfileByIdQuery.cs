using CQRSalad.Domain;
using CQRSalad.Infrastructure.Validation;
using FluentValidation;

namespace Samples.Domain.Interface.User
{
    public class UserProfileByIdQuery : IQueryFor<UserProfile>
    {
        public string UserId { get; set; }
    }

    public sealed class UserProfileByIdQueryValidator : FluentMessageValidator<UserProfileByIdQuery>
    {
        public UserProfileByIdQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
