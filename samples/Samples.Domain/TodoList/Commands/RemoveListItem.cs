using CQRSalad.Domain;
using CQRSalad.EventSourcing;

namespace Samples.Domain.Interface.TodoList.Commands
{
    public sealed class RemoveListItem : ICommand
    {
        [AggregateId]
        public string ListId { get; set; }
        public string ItemId { get; set; }
    }
}
