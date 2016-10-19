using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Interface.TodoList.Commands;

namespace Samples.Domain.Interface.Validation.TodoList.Commands
{
    public sealed class CompleteListItemValidator : FluentValidatorFor<CompleteListItem>
    {
        public CompleteListItemValidator()
        {
            RuleFor(x => x.ListId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
        }
    }
}
