using CQRSalad.EventStore.Core;

namespace Samples.Domain.Model.TodoList
{
    public sealed class TodoListDeleted : IEvent
    {
        public string ListId { get; set; }
    }
}