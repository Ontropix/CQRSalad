namespace Samples.View.Views
{
    public sealed class TodoListView : IView
    {
        public string Id { get; set; }
        public string Title { get; set; }
        public string OwnerId { get; set; }
    }
}