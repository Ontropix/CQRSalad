using System;
using CQRSalad.Infrastructure;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Samples.Domain.Specifications;

namespace Samples.Tests
{
    [TestClass]
    public class SpecificationRunnerTest
    {
        [TestMethod]
        public void Run()
        {
            SpecificationRunner.Run(new CreateUserSpecification("userId1", "user@mail.com"), Console.Out);
            SpecificationRunner.Run(new RemoveUserSpecification("userId1"), Console.Out);
            
            SpecificationRunner.Run(new CreateTodoListSpecification("list1", "Food", "user1"), Console.Out);
        }
    }
}
