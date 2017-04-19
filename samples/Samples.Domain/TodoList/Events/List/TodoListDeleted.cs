using CQRSalad.EventSourcing;

namespace Samples.Domain.TodoList
{
    public sealed class TodoListDeleted : IEvent
    {
        public string ListId { get; set; }
    }
}