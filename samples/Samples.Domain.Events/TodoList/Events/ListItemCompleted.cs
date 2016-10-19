using CQRSalad.EventStore.Core;

namespace Samples.Domain.Model.TodoList
{
    public sealed class ListItemCompleted : IEvent
    {
        public string ListId { get; set; }
        public string ItemId { get; set; }
    }
}
