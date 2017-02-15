using System.Collections.Generic;

namespace Samples.Domain.Model.TodoList
{
    public class TodoList
    {
        public string ListId { get; set; }
        public string Title { get; set; }

        public List<TodoListItem> Items { get; set; }
        public string OwnerId { get; set; }
    }
}