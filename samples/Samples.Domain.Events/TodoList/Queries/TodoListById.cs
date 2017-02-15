using CQRSalad.EventSourcing;

namespace Samples.Domain.Model.TodoList
{
    public class TodoListById : IQueryFor<TodoList>
    {
        public string ListId { get; set; }
    }
}
