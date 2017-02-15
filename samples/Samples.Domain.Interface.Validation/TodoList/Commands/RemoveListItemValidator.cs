using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Model.TodoList;

namespace Samples.Domain.Interface.Validation.TodoList.Commands
{
    public sealed class RemoveListItemValidator : FluentValidatorFor<RemoveListItem>
    {
        public RemoveListItemValidator()
        {
            RuleFor(x => x.ListId).NotEmpty();
            RuleFor(x => x.ItemId).NotEmpty();
        }
    }
}
