using System;
using StructureMap;

namespace Samples.Tests.Structuremap
{
    public class StructureMapServiceProvider : IServiceProvider
    {
        private readonly IContainer container;

        public StructureMapServiceProvider(IContainer container)
        {
            this.container = container;
        }

        public object GetService(Type serviceType)
        {
            return container.GetInstance(serviceType);
        }
    }
}
