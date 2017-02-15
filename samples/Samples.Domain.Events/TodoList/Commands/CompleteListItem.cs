using CQRSalad.EventSourcing;

namespace Samples.Domain.Model.TodoList
{
    public sealed class CompleteListItem : ICommand
    {
        [AggregateId]
        public string ListId { get; set; }
        public string ItemId { get; set; }
    }
}
