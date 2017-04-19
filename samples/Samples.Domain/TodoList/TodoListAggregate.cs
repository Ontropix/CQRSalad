using CQRSalad.EventSourcing;

namespace Samples.Domain.TodoList
{
    public class TodoListAggregate : AggregateRoot<TodoListState>
    {
        [AggregateConstructor]
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

        public void When(CompleteListItem command)
        {
            if (!State.ItemsIds.Contains(command.ItemId))
            {
                ProduceError("Item not found.");
            }

            ProduceEvent(command.MapToEvent<ListItemCompleted>());
        }

        public void When(RemoveListItem command)
        {
            if (!State.ItemsIds.Contains(command.ItemId))
            {
                ProduceError("Item not found.");
            }

            ProduceEvent(command.MapToEvent<ListItemRemoved>());
        }
    }
}