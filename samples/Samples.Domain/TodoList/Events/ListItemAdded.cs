using CQRSalad.EventSourcing;

namespace Samples.Domain.TodoList
{
    public sealed class ListItemAdded : IEvent
    {
        public string ListId { get; set; }
        public string ItemId { get; set; }
        public string Description { get; set; }
    }
}