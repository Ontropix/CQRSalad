using CQRSalad.EventStore.Core;

namespace Samples.Domain.Model.TodoList
{
    public sealed class TodoListCreated : IEvent
    {
        public string ListId { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }
    }
}