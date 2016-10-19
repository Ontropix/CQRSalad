using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Interface.User;

namespace Samples.Domain.Interface.Validation.User.Commands
{
    public sealed class CreateUserCommandValidator : FluentValidatorFor<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
        }
    }
}