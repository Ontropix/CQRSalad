using CQRSalad.EventSourcing;

namespace Samples.Domain.Interface.TodoList.Commands
{
    public sealed class CompleteListItem : ICommand
    {
        [AggregateId]
        public string ListId { get; set; }
        public string ItemId { get; set; }
    }
}
