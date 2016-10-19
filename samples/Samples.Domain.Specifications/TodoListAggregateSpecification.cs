using System;
using CQRSalad.EventSourcing.Testing;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using Samples.Domain.Interface.TodoList.Commands;
using Samples.Domain.Model.TodoList;
using Samples.Domain.Model.User;

namespace Samples.Domain.Specifications
{
    internal static class TodoListScenarios
    {
        internal static UserCreated UserCreatedEvent(string userId, string email, string userName)
        {
            return new UserCreated
            {
                UserId = userId,
                Email = email
            };
        }
    }

    [TestClass]
    public class TodoListAggregateSpecification : AggregateSpecification<TodoListAggregate>
    {
        [TestMethod]
        public void When_CreateTodoList_Command_ShouldBe_TodoListCreated_Event()
        {
            string listId = Guid.NewGuid().ToString();
            string listTitle = "Food";
            string ownerId = Guid.NewGuid().ToString();

            Given();

            When(new CreateTodoList
            {
                ListId = listId,
                Title = listTitle,
                OwnerId = ownerId
            });

            Expected(new TodoListCreated
            {
                ListId = listId,
                Title = listTitle,
                OwnerId = ownerId
            });
        }
    }
}
