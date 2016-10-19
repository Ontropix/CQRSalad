using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Interface.TodoList.Queries;

namespace Samples.Domain.Interface.Validation.TodoList.Queries
{
    public sealed class TodoListsByOwnerIdValidator : FluentValidatorFor<TodoListsByOwnerId>
    {
        public TodoListsByOwnerIdValidator()
        {
            RuleFor(x => x.ListId).NotEmpty();
        }
    }
}