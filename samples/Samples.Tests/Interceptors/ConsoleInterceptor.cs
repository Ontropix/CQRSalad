using System;
using System.Threading.Tasks;
using CQRSalad.Dispatching;
using Newtonsoft.Json;

namespace Samples.Tests.Configurators
{
    public class ConsoleInterceptor : IContextInterceptor
    {
        public async Task OnExecuting(DispatchingContext context)
        {
            await Task.CompletedTask;
            Console.WriteLine($"MESSAGE TYPE     : {context.MessageInstance.GetType()}");
            Console.WriteLine($"MESSAGE INSTANCE : {JsonConvert.SerializeObject(context.MessageInstance)}");
            Console.WriteLine($"HANDLER TYPE     : {context.HandlerInstance.GetType()}");
        }

        public async Task OnExecuted(DispatchingContext context)
        {
            await Task.CompletedTask;
            Console.WriteLine($"RESULT: {JsonConvert.SerializeObject(context.Result)}");
            Console.WriteLine();
        }

        public async Task OnException(DispatchingContext context, Exception invocationException)
        {
            await Task.CompletedTask;
        }
    }
}