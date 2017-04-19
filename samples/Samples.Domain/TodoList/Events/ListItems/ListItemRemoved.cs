using CQRSalad.EventSourcing;

namespace Samples.Domain.TodoList
{
    public sealed class ListItemRemoved : IEvent
    {
        public string ListId { get; set; }
        public string ItemId { get; set; }
    }
}
