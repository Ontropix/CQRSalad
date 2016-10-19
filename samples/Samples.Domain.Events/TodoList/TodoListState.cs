using System.Collections.Generic;

namespace Samples.Domain.Model.TodoList
{
    public class TodoListState
    {
        public HashSet<string> ItemsIds { get; set; } = new HashSet<string>();
    }
}