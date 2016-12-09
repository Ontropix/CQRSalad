using System;
using CQRSalad.EventSourcing.Specification;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Samples.Domain.Specifications
{
    [TestClass]
    public class SpecificationRunnerTest
    {
        [TestMethod]
        public void Run()
        {
            SpecificationRunner.Run(new CreateUserSpecification("userId1", "user@mail.com"), Console.Out);
            SpecificationRunner.Run(new CreateTodoListSpecification("list1", "Food", "user1"), Console.Out);
        }
    }
}
