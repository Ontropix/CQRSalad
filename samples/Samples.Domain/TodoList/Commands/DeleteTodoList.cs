using CQRSalad.Domain;
using CQRSalad.EventSourcing;

namespace Samples.Domain.Interface.TodoList.Commands
{
    public sealed class DeleteTodoList : ICommand
    {
        [AggregateId]
        public string ListId { get; set; }
    }
}