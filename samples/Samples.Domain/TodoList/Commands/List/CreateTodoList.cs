using CQRSalad.EventSourcing;
using FluentValidation;

namespace Samples.Domain.TodoList
{
    public sealed class CreateTodoList : ICommand
    {
        [AggregateId]
        public string ListId { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }
    }

    public sealed class CreateTodoListValidator : AbstractValidator<CreateTodoList>
    {
        public CreateTodoListValidator()
        {
            RuleFor(x => x.ListId).NotEmpty();
            RuleFor(x => x.Title).NotEmpty();
            RuleFor(x => x.OwnerId).NotEmpty();
        }
    }
}