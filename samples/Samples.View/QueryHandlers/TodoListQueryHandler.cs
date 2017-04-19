using System.Threading.Tasks;
using CQRSalad.Dispatching;
using Kutcha.Core;
using Samples.Domain.TodoList;
using Samples.ViewModel.Views;

namespace Samples.ViewModel.QueryHandlers
{
    [DispatcherHandler]
    public class TodoListQueryHandler
    {
        private readonly IKutchaReadOnlyStore<TodoListView> _store;

        public TodoListQueryHandler(IKutchaReadOnlyStore<TodoListView> store)
        {
            _store = store;
        }

        public async Task<TodoList> Query(TodoListById query)
        {
            TodoListView view = await _store.FindByIdAsync(query.ListId);
            var list = new TodoList
            {
                ListId = view.Id,
                Title = view.Title,
                OwnerId = view.OwnerId
            };

            return list;
        }
    }
}