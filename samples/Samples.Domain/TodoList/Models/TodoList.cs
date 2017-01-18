using System.Collections.Generic;
using Samples.Domain.Interface.TodoList.Models;

namespace Samples.Domain.Interface.TodoList
{
    public class TodoList
    {
        public string ListId { get; set; }
        public string Title { get; set; }

        public List<TodoListItem> Items { get; set; }
        public string OwnerId { get; set; }
    }
}