using System.Collections.Generic;
using CQRSalad.Infrastructure;

namespace Samples.Domain.TodoList
{
    public sealed class TodoListsByOwnerId : IQueryFor<List<TodoListSummary>>
    {
        public string OwnerId { get; set; }
    }
}