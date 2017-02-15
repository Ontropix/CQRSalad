using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Model.TodoList;

namespace Samples.Domain.Interface.Validation.TodoList.Commands
{
    public sealed class DeleteTodoListValidator : FluentValidatorFor<DeleteTodoList>
    {
        public DeleteTodoListValidator()
        {
            RuleFor(x => x.ListId).NotEmpty();
        }
    }
}