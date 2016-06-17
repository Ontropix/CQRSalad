using System;
using System.Threading.Tasks;

namespace CQRSalad.Tests.Dispatcher
{
    public class TestCommand
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
    }

    public class TestHandler
    {
        public void Handle(TestCommand message)
        {
        }

        public string HandleWithResponse(TestCommand command)
        {
            return "Handled";
        }

        public async Task HandleAsync(TestCommand message)
        {
            await Task.CompletedTask;
        }
        
        public async Task<string> HandleWithResponseAsync(TestCommand message)
        {
            return await Task.FromResult("HandledAsync");
        }
    }
}