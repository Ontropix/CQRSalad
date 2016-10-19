using System.Collections.Generic;
using CQRSalad.Domain;
using Samples.Domain.Interface.TodoList.Models;

namespace Samples.Domain.Interface.TodoList.Queries
{
    public sealed class TodoListsByOwnerId : IQueryFor<List<TodoListSummary>>
    {
        public string ListId { get; set; }
    }
}