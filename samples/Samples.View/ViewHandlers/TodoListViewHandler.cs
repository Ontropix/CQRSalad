using System.Threading.Tasks;
using CQRSalad.Dispatching;
using CQRSalad.Dispatching.Priority;
using Kutcha.Core;
using Samples.Domain.Model.TodoList;
using Samples.View.Views;

namespace Samples.View.ViewHandlers
{
    [DispatcherHandler]
    [DispatchingPriority(DispatchingPriority.High)]
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
                OwnerId = evnt.OwnerId
            });
        }

        public async Task Apply(TodoListDeleted evnt)
        {
        }

        public async Task Apply(ListItemAdded evnt)
        {
        }

        public async Task Apply(ListItemRemoved evnt)
        {
        }

        public async Task Apply(ListItemCompleted evnt)
        {
        }
    }
}