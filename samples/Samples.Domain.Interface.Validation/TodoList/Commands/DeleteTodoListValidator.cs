using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Interface.TodoList.Commands;

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