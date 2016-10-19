using CQRSalad.Domain;
using CQRSalad.EventSourcing;

namespace Samples.Domain.Interface.TodoList.Commands
{
    public sealed class CreateTodoList : ICommand
    {
        [AggregateId]
        public string ListId { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }
    }
}