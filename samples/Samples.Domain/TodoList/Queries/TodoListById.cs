using CQRSalad.Domain;

namespace Samples.Domain.Interface.TodoList.Queries
{
    public class TodoListById : IQueryFor<TodoList>
    {
        public string ListId { get; set; }
    }
}
