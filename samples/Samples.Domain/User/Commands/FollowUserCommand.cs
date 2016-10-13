using CQRSalad.Domain;
using CQRSalad.EventSourcing;
using CQRSalad.Infrastructure.Validation;
using FluentValidation;

namespace Samples.Domain.Interface.User
{
    public sealed class FollowUserCommand : ICommand
    {
        [AggregateId]
        public string UserId { get; set; }
        public string FollowingUserId { get; set; }
    }

    public sealed class FollowUserCommandValidator : FluentMessageValidator<FollowUserCommand>
    {
        public FollowUserCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.FollowingUserId).NotEmpty();
        }
    }
}