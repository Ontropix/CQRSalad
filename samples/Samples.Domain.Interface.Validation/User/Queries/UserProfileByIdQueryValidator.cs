using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Model.User;

namespace Samples.Domain.Interface.Validation.User.Queries
{ 
    public sealed class UserProfileByIdQueryValidator : FluentValidatorFor<UserProfileById>
    {
        public UserProfileByIdQueryValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
        }
    }
}
