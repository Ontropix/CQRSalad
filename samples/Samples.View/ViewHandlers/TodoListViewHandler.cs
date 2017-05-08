using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using CQRSalad.Dispatching;
using Dapper;
using Kutcha.Core;
using Npgsql;
using Samples.Domain.TodoList;
using Samples.ViewModel.Views;
using TodoListItem = Samples.ViewModel.Views.TodoListItem;

namespace Samples.ViewModel.ViewHandlers
{
    [DispatcherHandler]
    [DispatchingPriority(Priority.High)]
    public sealed class TodoListViewHandler
    {
        private readonly IKutchaStore<TodoListView> _store;

        public TodoListViewHandler(IKutchaStore<TodoListView> store)
        {
            _store = store;
        }

        public async Task Apply(TodoListCreated evnt)
        {
            await _store.InsertAsync(new TodoListView
            {
                Id = evnt.ListId,
                Title = evnt.Title,
                OwnerId = evnt.OwnerId,
                Items = new Dictionary<string, TodoListItem>()
            });
        }

        public async Task Apply(TodoListDeleted evnt)
        {
            await _store.DeleteByIdAsync(evnt.ListId);
        }

        public async Task Apply(ListItemAdded evnt)
        {
            await _store.FindOneAndUpdateAsync(
                evnt.ListId,
                x => x.Items.Add(evnt.ItemId, new TodoListItem
                {
                    Id = evnt.ItemId,
                    Description = evnt.Description,
                    Status = TodoItemStatus.Added
                })
            );
        }

        public async Task Apply(ListItemRemoved evnt)
        {
            await _store.FindOneAndUpdateAsync(
                evnt.ListId,
                x => x.Items.Remove(evnt.ItemId));
        }

        public async Task Apply(ListItemCompleted evnt)
        {
            await _store.FindOneAndUpdateAsync(
                   evnt.ListId,
                   x => x.Items[evnt.ItemId].Status = TodoItemStatus.Completed
               );
        }
    }

    [DispatcherHandler]
    [DispatchingPriority(Priority.Normal)]
    public sealed class TodoListViewHandlerPostgre
    {
        private readonly string _connectionString = "User ID=postgres;Password=7799;Host=localhost;Port=5432;Database=cqrsalad;Pooling=true;";

        public async Task Apply(TodoListCreated evnt)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();

                int result = await dbConnection.ExecuteAsync(
                    "INSERT INTO todolists (id, title, ownerid) " +
                    "VALUES (@Id, @Title, @OwnerId)",
                    new
                    {
                        Id = Guid.Parse(evnt.ListId),
                        Title = evnt.Title,
                        OwnerId = Guid.Parse(evnt.OwnerId)
                    });
            }
        }

        public async Task Apply(TodoListDeleted evnt)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                int result = await dbConnection.ExecuteAsync(
                    "DELETE FROM todolists WHERE id = @Id",
                    new { Id = Guid.Parse(evnt.ListId) });
            }
        }

        public async Task Apply(ListItemAdded evnt)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();

                int result = await dbConnection.ExecuteAsync(
                    "INSERT INTO todoitems (id, description, Status, listid) " +
                    "VALUES (@Id, @Description, @Status, @ListId)",
                    new
                    {
                        Id = Guid.Parse(evnt.ItemId),
                        Description = evnt.Description,
                        Status = TodoItemStatus.Added,
                        ListId = Guid.Parse(evnt.ListId)
                    });
            }
        }

        public async Task Apply(ListItemRemoved evnt)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                int result = await dbConnection.ExecuteAsync(
                    "DELETE FROM todoitems WHERE id = @Id",
                    new { Id = Guid.Parse(evnt.ItemId) });
            }
        }

        public async Task Apply(ListItemCompleted evnt)
        {
            using (IDbConnection dbConnection = new NpgsqlConnection(_connectionString))
            {
                dbConnection.Open();
                int result = await dbConnection.ExecuteAsync(
                    "UPDATE todoitems " +
                    "SET Status = @Status " +
                    "WHERE Id = @Id",
                    new
                    {
                        Id = Guid.Parse(evnt.ItemId),
                        Status = TodoItemStatus.Completed
                    });
            }
        }
    }
}