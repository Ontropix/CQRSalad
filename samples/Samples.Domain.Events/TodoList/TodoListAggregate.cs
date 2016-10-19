using CQRSalad.EventSourcing;
using Samples.Domain.Interface.TodoList.Commands;

namespace Samples.Domain.Model.TodoList
{
    public class TodoListAggregate : AggregateRoot<TodoListState>
    {
        [AggregateCtor]
        public void When(CreateTodoList command)
        {
            ProduceEvent(command.MapTo<TodoListCreated>());
        }
        
        public void When(DeleteTodoList command)
        {
            ProduceEvent(command.MapTo<TodoListDeleted>());
        }

        public void When(AddListItem command)
        {
            if (State.ItemsIds.Contains(command.ItemId))
            {
                ProduceError("Duplicate item ID.");
            }

            ProduceEvent(command.MapTo<ListItemAdded>());
        }

        public void When(RemoveListItem command)
        {
            if (!State.ItemsIds.Contains(command.ItemId))
            {
                ProduceError("Item not found.");
            }

            ProduceEvent(command.MapTo<ListItemRemoved>());
        }

        public void When(CompleteListItem command)
        {
            if (!State.ItemsIds.Contains(command.ItemId))
            {
                ProduceError("Item not found.");
            }

            ProduceEvent(command.MapTo<ListItemCompleted>());
        }
    }
}