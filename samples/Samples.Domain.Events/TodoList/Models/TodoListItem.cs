namespace Samples.Domain.Model.TodoList
{
    public class TodoListItem
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public TodoItemStatus Status { get; set; }
    }
}