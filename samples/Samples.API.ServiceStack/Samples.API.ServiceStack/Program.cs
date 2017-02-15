using System;
using System.Diagnostics;
using Samples.Tests.Configurators;
using ServiceStack.Configuration;
using ServiceStack.Text;

namespace Samples.API.ServiceStack
{
    public class StructureMapContainerAdapter : IContainerAdapter
    {
        private readonly StructureMap.IContainer _container;

        public StructureMapContainerAdapter(StructureMap.IContainer container)
        {
            _container = container;
        }

        public T TryResolve<T>()
        {
            return _container.TryGetInstance<T>();
        }

        public T Resolve<T>()
        {
            return _container.TryGetInstance<T>();
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            var container = new StructureMap.Container()
                .UseGuidIdGenerator()
                .UseInMemoryKutcha()
                .RegisterKutchaRoots()
                .UseDispatcher()
                .UseInMemoryBuses()
                .UseInMemoryEventStore()
                .UseCommandProcessorSingleton()
                .UseFluentMessageValidator();

            var host =new AppHost().Init().Start("http://*:8088/");
            host.Container.Adapter = new StructureMapContainerAdapter(container);

            "ServiceStack Self Host with Razor listening at http://127.0.0.1:8088".Print();

            Process.Start("http://127.0.0.1:8088/");

            Console.ReadLine();
        }
    }
}
