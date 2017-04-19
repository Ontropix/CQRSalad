using System.Collections.Generic;

namespace Samples.Domain.TodoList
{
    public class TodoListState
    {
        internal bool IsDeleted { get; set; }
        internal HashSet<string> ItemsIds { get; set; } = new HashSet<string>();

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