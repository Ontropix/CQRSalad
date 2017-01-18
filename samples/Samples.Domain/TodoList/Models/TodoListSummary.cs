namespace Samples.Domain.Interface.TodoList.Models
{
    public class TodoListSummary
    {
        public string ListId { get; set; }
        public string Title { get; set; }
        public string ItemsCount { get; set; }
        public string OwnerId { get; set; }
    }
}
