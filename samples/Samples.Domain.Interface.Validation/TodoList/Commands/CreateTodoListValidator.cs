using CQRSalad.Infrastructure.Validation;
using FluentValidation;
using Samples.Domain.Interface.TodoList.Commands;

namespace Samples.Domain.Interface.Validation.TodoList.Commands
{
    public sealed class CreateTodoListValidator : FluentValidatorFor<CreateTodoList>
    {
        public CreateTodoListValidator()
        {
            RuleFor(x => x.ListId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.OwnerId).NotEmpty();
        }
    }
}