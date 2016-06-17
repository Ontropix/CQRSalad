using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace CQRSalad.Tests.Dispatcher
{
    [TestClass]
    public class UnitTest1
    {
        public UnitTest1()
        {
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            var executor = GetExecutor(typeof(TestHandler), typeof(FileStream), typeof(TestHandler).GetMethod("Handle2"));
            var voidExecutor = GetExecutor(typeof(TestHandler), typeof(FileStream), typeof(TestHandler).GetMethod("Handle"));

            var asyncExecutor = GetExecutor(typeof(TestHandler), typeof(FileStream), typeof(TestHandler).GetMethod("Handle2Async"));

            object res = executor(new TestHandler(), new FileStream(@"d:\ssor.doc", FileMode.Open));
            object voidRes = voidExecutor(new TestHandler(), new FileStream(@"d:\ssor.doc", FileMode.Open));
            Task resAsync = (Task)asyncExecutor(new TestHandler(), new FileStream(@"d:\ssor.doc", FileMode.Open));
            await resAsync;
            string result = (string) GetTaskResult(resAsync);
        }

        private object GetTaskResult(Task task)
        {
            return task.GetType().GetProperty("Result").GetValue(task);
        }
        
        private static HandlerExecutor GetExecutor(Type handlerType, Type messageType, MethodInfo action)
        {
            Type objectType = typeof (object);
            ParameterExpression handlerParameter = Expression.Parameter(objectType, "handler");
            ParameterExpression messageParameter = Expression.Parameter(objectType, "message");

            MethodCallExpression methodCall =
                Expression.Call(
                    Expression.Convert(handlerParameter, handlerType),
                    action,
                    Expression.Convert(messageParameter, messageType));

            if (action.ReturnType == typeof (void))
            {
                var lambda = Expression.Lambda<Action<object, object>>(
                    methodCall,
                    handlerParameter,
                    messageParameter);

                Action<object, object> voidExecutor = lambda.Compile();
                return (handler, message) =>
                {
                    voidExecutor(handler, message);
                    return null;
                };
            }
            else
            {
                var lambda = Expression.Lambda<HandlerExecutor>(
                    Expression.Convert(methodCall, typeof(object)),
                    handlerParameter,
                    messageParameter);

                return lambda.Compile();
            }
        }
       
        internal delegate object HandlerExecutor(object handler, object message);
    }
}
