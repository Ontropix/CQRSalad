using System;
using System.Threading.Tasks;

namespace CQRSalad.Infrastructure
{
    internal static class Maybe
    {
        public static TResult With<TInput, TResult>(this TInput obj, Func<TInput, TResult> evaluator)
            where TInput : class
            where TResult : class
        {
            if (obj == null)
            {
                return null;
            }
            return evaluator(obj);
        }

        public static TResult Cast<TInput, TResult>(this TInput obj)
            where TInput : class
            where TResult : class
        {
            return obj as TResult;
        }

        public static TResult Cast<TResult>(this object obj)
            where TResult : class
        {
            return obj as TResult;
        }
        
        public static TResult Return<TInput, TResult>(this TInput obj, Func<TInput, TResult> evaluator, TResult failureValue)
            where TInput : class
        {
            if (obj == null)
            {
                return failureValue;
            }
            return evaluator(obj);
        }

        public static TInput Do<TInput>(this TInput obj, Action<TInput> action)
            where TInput : class
        {
            if (obj == null)
            {
                return null;
            }

            action(obj);
            return obj;
        }

        public static async Task<TInput> Do<TInput>(this TInput obj, Func<TInput, Task> action)
            where TInput : class
        {
            if (obj == null)
            {
                return null;
            }

            await action(obj);
            return obj;
        }

        public static bool ReturnSuccess<TInput>(this TInput obj)
            where TInput : class 
        {
            return obj != null;
        }

        public static TInput If<TInput>(this TInput obj, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (obj == null)
            {
                return null;
            }
            return evaluator(obj) ? obj : null;
        }

        public static TInput Unless<TInput>(this TInput obj, Func<TInput, bool> evaluator)
            where TInput : class
        {
            if (obj == null)
            {
                return null;
            }
            return evaluator(obj) ? null : obj;
        }
    }
}
