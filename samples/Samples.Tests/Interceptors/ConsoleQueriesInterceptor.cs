using System;
using System.Threading.Tasks;
using CQRSalad.Dispatching;
using CQRSalad.Infrastructure.Interceptors;
using Newtonsoft.Json;

namespace Samples.Tests.Configurators
{
    public class ConsoleQueriesInterceptor : QueriesInterceptor
    {
        public override async Task OnExecuting(DispatchingContext context)
        {
            await Task.CompletedTask;
            Console.WriteLine($"MESSAGE TYPE     : {context.MessageInstance.GetType()}");
            Console.WriteLine($"MESSAGE INSTANCE : {JsonConvert.SerializeObject(context.MessageInstance)}");
            Console.WriteLine($"HANDLER TYPE     : {context.HandlerInstance.GetType()}");
        }

        public override async Task OnExecuted(DispatchingContext context)
        {
            await Task.CompletedTask;
            Console.WriteLine($"RESULT: {JsonConvert.SerializeObject(context.Result)}");
            Console.WriteLine();
        }

        public override async Task OnException(DispatchingContext context)
        {
            await Task.CompletedTask;
        }
    }
}