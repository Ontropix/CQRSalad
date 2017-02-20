using System;
using System.Threading.Tasks;

namespace CQRSalad.Dispatching
{
    internal static class TaskExtensions
    {
        internal static object GetTaskResult(this Task task)
        {
            //todo type checking
            //todo caching
            return task.GetType().GetProperty("Result")?.GetValue(task);
        }

        //public static object GetResult(this Task task)
        //{
        //    try
        //    {
        //        task.Wait();

        //        Type taskType = task.GetType();
        //        if (!taskType.IsGenericType || taskType.FullName.Contains("VoidTaskResult"))
        //        {
        //            return null;
        //        }

        //        var fn = taskType.GetFastGetter("Result");
        //        return fn?.Invoke(task);
        //    }
        //    catch (TypeAccessException)
        //    {
        //        return null; //return null for void Task's
        //    }
        //    catch (Exception ex)
        //    {
        //       // throw ex.UnwrapIfSingleException();
        //    }
        //}
    }
}