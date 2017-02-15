using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Model.TodoList;

namespace Samples.Domain.Interface.Validation.TodoList.Queries
{
    public sealed class TodoListByIdValidator : FluentValidatorFor<TodoListById>
    {
        public TodoListByIdValidator()
        {
            RuleFor(x => x.ListId).NotEmpty();
        }
    }
}
