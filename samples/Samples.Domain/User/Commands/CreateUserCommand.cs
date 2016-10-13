using CQRSalad.Domain;
using CQRSalad.EventSourcing;
using CQRSalad.Infrastructure.Validation;
using FluentValidation;

namespace Samples.Domain.Interface.User
{
    public class CreateUserCommand : ICommand
    {
        [AggregateId]
        public string UserId { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
    }

    public sealed class CreateUserCommandValidator : FluentMessageValidator<CreateUserCommand>
    {
        public CreateUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.Email).NotEmpty();
            RuleFor(x => x.UserName).NotEmpty();
        }
    }
}