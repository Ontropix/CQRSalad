using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    internal static class TaskExtensions
    {
        internal static object GetTaskResult(this Task task)
        {
            //todo type checking
            //todo caching
            return task.GetType().GetProperty("Result").GetValue(task);
        }
    }
}