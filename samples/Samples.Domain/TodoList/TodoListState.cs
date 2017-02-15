using System.Collections.Generic;
using Samples.Domain.Model.TodoList;

namespace Samples.Domain.Interface.TodoList
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