using System.Collections.Generic;

namespace Samples.Domain.Model.TodoList
{
    public class TodoListState
    {
        public HashSet<string> ItemsIds { get; set; } = new HashSet<string>();

        public void On(ListItemAdded evnt)
        {
            ItemsIds.Add(evnt.ItemId);
        }

        public void On(ListItemRemoved evnt)
        {
            ItemsIds.Remove(evnt.ItemId);
        }

        public void On(ListItemCompleted evnt)
        {
        }
    }
}