using CQRSalad.Infrastructure;

namespace Samples.Domain.TodoList
{
    public class TodoListById : IQueryFor<TodoList>
    {
        public string ListId { get; set; }
    }
}
