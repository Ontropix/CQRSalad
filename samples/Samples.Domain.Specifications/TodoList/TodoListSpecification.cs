using System.Collections.Generic;
using CQRSalad.EventSourcing;
using CQRSalad.EventSourcing.Specifications;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samples.Domain.Interface.TodoList;
using Samples.Domain.Model.TodoList;

namespace Samples.Domain.Specifications
{
    [TestClass]
    public class CreateTodoListSpecification : AggregateSpecification<TodoListAggregate>
    {
        private readonly string _id;
        private readonly string _title;
        private readonly string _ownerId;

        public CreateTodoListSpecification(string id, string title, string ownerId)
        {
            _id = id;
            _title = title;
            _ownerId = ownerId;
        }

        public override ICommand When()
        {
            return new CreateTodoList
            {
                ListId = _id,
                Title = _title,
                OwnerId = _ownerId
            };
        }

        public override IEnumerable<IEvent> Expected()
        {
            yield return new TodoListCreated
            {
                ListId = _id,
                Title = _title,
                OwnerId = _ownerId
            };
        }
    }
}
