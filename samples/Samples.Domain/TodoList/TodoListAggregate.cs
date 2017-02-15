using CQRSalad.EventSourcing;
using Samples.Domain.Model.TodoList;

namespace Samples.Domain.Interface.TodoList
{
    public class TodoListAggregate : AggregateRoot<TodoListState>
    {
        [AggregateCtor]
        public void When(CreateTodoList command)
        {
            ProduceEvent(command.MapToEvent<TodoListCreated>());
        }
        
        public void When(DeleteTodoList command)
        {
            ProduceEvent(command.MapToEvent<TodoListDeleted>());
        }

        public void When(AddListItem command)
        {
            if (State.ItemsIds.Contains(command.ItemId))
            {
                ProduceError("Duplicate item ID.");
            }

            ProduceEvent(command.MapToEvent<ListItemAdded>());
        }

        public void When(RemoveListItem command)
        {
            if (!State.ItemsIds.Contains(command.ItemId))
            {
                ProduceError("Item not found.");
            }

            ProduceEvent(command.MapToEvent<ListItemRemoved>());
        }

        public void When(CompleteListItem command)
        {
            if (!State.ItemsIds.Contains(command.ItemId))
            {
                ProduceError("Item not found.");
            }

            ProduceEvent(command.MapToEvent<ListItemCompleted>());
        }
    }
}