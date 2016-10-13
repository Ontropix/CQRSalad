using CQRSalad.Domain;
using CQRSalad.EventSourcing;
using CQRSalad.Infrastructure.Validation;
using FluentValidation;

namespace Samples.Domain.Interface.User
{
    public sealed class AddFollowerCommand : ICommand
    {
        [AggregateId]
        public string UserId { get; set; }
        public string FollowerUserId { get; set; }
    }

    public sealed class AddFollowerCommandValidator : FluentMessageValidator<AddFollowerCommand>
    {
        public AddFollowerCommandValidator()
        {
            RuleFor(x => x.UserId).NotEmpty();
            RuleFor(x => x.FollowerUserId).NotEmpty();
        }
    }
}