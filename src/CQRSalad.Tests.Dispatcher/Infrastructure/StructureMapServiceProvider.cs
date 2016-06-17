using System;
using StructureMap;

namespace CQRSalad.Tests.Dispatcher.Infrastructure
{
    public class StructureMapServiceProvider : IServiceProvider
    {
        private readonly IContainer _container;

        public StructureMapServiceProvider(IContainer container)
        {
            _container = container;
        }

        public object GetService(Type serviceType)
        {
            return _container.GetInstance(serviceType);
        }
    }
}
