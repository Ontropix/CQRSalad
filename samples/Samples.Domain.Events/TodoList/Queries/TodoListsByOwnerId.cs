using System.Collections.Generic;
using CQRSalad.EventSourcing;

namespace Samples.Domain.Model.TodoList
{
    public sealed class TodoListsByOwnerId : IQueryFor<List<TodoListSummary>>
    {
        public string OwnerId { get; set; }
    }
}