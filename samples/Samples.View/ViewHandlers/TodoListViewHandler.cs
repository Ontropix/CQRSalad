using System.Collections.Generic;
using System.Threading.Tasks;
using CQRSalad.Dispatching;
using Kutcha.Core;
using Samples.Domain.Model.TodoList;
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
}