using CQRSalad.EventStore.Core;

namespace Samples.Domain.Model.TodoList
{
    public sealed class ListItemAdded : IEvent
    {
        public string ListId { get; set; }
        public string ItemId { get; set; }
        public string Description { get; set; }
    }
}