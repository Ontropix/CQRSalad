using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Interface.User;

namespace Samples.Domain.Interface.Validation.User.Queries
{ 
    public sealed class UserProfileByIdQueryValidator : FluentValidatorFor<UserProfileByIdQuery>
    {
        public UserProfileByIdQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
