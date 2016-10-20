using System.Collections.Generic;
using Samples.Domain.Interface.TodoList;

namespace Samples.View.Views
{
    public sealed class TodoListView : IView
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }

        /// <summary>
        /// Key: Id, Value: Item
        /// </summary>
        public Dictionary<string ,TodoListItem> Items { get; set; }
    }

    public sealed class TodoListItem
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public TodoItemStatus Status { get; set; }
    }
}