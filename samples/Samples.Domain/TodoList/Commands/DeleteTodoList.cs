using CQRSalad.EventSourcing;

namespace Samples.Domain.TodoList
{
    public sealed class DeleteTodoList : ICommand
    {
        [AggregateId]
        public string ListId { get; set; }
    }
}