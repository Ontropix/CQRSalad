using System.Collections.Generic;
using CQRSalad.EventSourcing;
using Samples.Domain.Interface.TodoList.Models;

namespace Samples.Domain.Interface.TodoList.Queries
{
    public sealed class TodoListsByOwnerId : IQueryFor<List<TodoListSummary>>
    {
        public string OwnerId { get; set; }
    }
}