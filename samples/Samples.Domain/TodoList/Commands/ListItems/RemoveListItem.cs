using CQRSalad.EventSourcing;

namespace Samples.Domain.TodoList
{
    public sealed class RemoveListItem : ICommand
    {
        [AggregateId]
        public string ListId { get; set; }
        public string ItemId { get; set; }
    }
}
