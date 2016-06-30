using System;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using CQRSalad.Dispatching.NEW.Core;
using CQRSalad.Dispatching.NEW.Descriptors;
using CQRSalad.Dispatching.NEW.Subscriptions;
using CQRSalad.Tests.Dispatcher;
using CQRSalad.Tests.Dispatcher.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;

namespace CQRSalad.Tests.Dispatcher2
{
    [TestClass]
    public class Unit2
    {
        private readonly Dispatching.NEW.Core.Dispatcher _dispatcher;

        public Unit2()
        {
            var container = new Container(expression =>
            {
                expression.For<TestHandler>().Use<TestHandler>();
            });

            _dispatcher = CQRSalad.Dispatching.NEW.Core.Dispatcher.Create(configuration =>
            {
                configuration.ServiceProvider = new DefaultDispatcherServiceProvider(new StructureMapServiceProvider(container));
                //configuration.SubscriptionsStore = new DispatcherSubscriptionsManager(new DefaultDispatcherHandlerDescriptorsBuilder(), );
            });
        }

        [TestMethod]
        public async Task TestMethod1()
        {
            object res1 = await _dispatcher.SendAsync(new TestCommand { Id = Guid.NewGuid(), Name = "First", Count = 1});
            object res2 = await _dispatcher.SendAsync(new TestCommand { Id = Guid.NewGuid(), Name = "Second", Count = 2});
            object res3 = await _dispatcher.SendAsync(new TestCommand { Id = Guid.NewGuid(), Name = "Third", Count = 3});
            object res4 = await _dispatcher.SendAsync(new TestCommand { Id = Guid.NewGuid(), Name = "Fourth", Count = 4});
        }
    }
}
