using CQRSalad.EventSourcing;

namespace Samples.Domain.Interface.TodoList.Commands
{
    public sealed class AddListItem : ICommand
    {
        [AggregateId]
        public string ListId { get; set; }
        public string ItemId { get; set; }
        public string Description { get; set; }
    }
}