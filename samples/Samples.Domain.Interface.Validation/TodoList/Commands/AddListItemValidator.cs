using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Interface.TodoList.Commands;

namespace Samples.Domain.Interface.Validation.TodoList.Commands
{
    public sealed class AddListItemValidator : FluentValidatorFor<AddListItem>
    {
        public AddListItemValidator()
        {
            RuleFor(x => x.ListId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
            RuleFor(x => x.Description).NotEmpty();
        }
    }
}