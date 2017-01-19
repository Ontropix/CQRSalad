using System;
using NUnit.Framework;
using Samples.API.ServiceStack.ServiceInterface;
using Samples.API.ServiceStack.ServiceModel;
using ServiceStack.Testing;
using ServiceStack;

namespace Samples.API.ServiceStack.Tests
{
    [TestFixture]
    public class UnitTests
    {
        private readonly ServiceStackHost appHost;

        public UnitTests()
        {
            appHost = new BasicAppHost(typeof(TodoService).Assembly)
            {
                ConfigureContainer = container =>
                {
                    //Add your IoC dependencies here
                }
            }
            .Init();
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            appHost.Dispose();
        }

        [Test]
        public void TestMethod1()
        {
            var service = appHost.Container.Resolve<TodoService>();

            //var response = (CreateTodoListResponse)service.Any(new CreateTodoListRequest { Name = "World" });

            //Assert.That(response.Result, Is.EqualTo("Hello, World!"));
        }
    }
}
