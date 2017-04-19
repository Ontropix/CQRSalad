using System.Collections.Generic;

namespace Samples.Domain.TodoList
{
    public sealed class TodoListState
    {
        internal bool IsDeleted { get; private set; }
        internal HashSet<string> ItemsIds { get; set; } = new HashSet<string>();

        public void On(TodoListDeleted evnt)
        {
            IsDeleted = true;
        }
        
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