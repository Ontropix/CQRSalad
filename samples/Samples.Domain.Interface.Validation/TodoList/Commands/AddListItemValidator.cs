using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Model.TodoList;

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