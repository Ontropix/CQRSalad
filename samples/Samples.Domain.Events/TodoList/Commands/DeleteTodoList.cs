using CQRSalad.EventSourcing;

namespace Samples.Domain.Model.TodoList
{
    public sealed class DeleteTodoList : ICommand
    {
        [AggregateId]
        public string ListId { get; set; }
    }
}