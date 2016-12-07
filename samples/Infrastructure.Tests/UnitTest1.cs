using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Infrastructure.Tests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestReflection()
        {
            string id = GetAggregateId(new AddListItem
            {
                ListId = Guid.NewGuid().ToString(),
                ItemId = Guid.NewGuid().ToString(),
                Description = "Descriptioonnn"
            });
        }

        private string GetAggregateId<TCommand>(TCommand command)
        {
            List<PropertyInfo> propertiesWithAggregateId =
                command
                    .GetType()
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(prop => prop.IsDefined(typeof(AggregateIdAttribute), false))
                    .ToList();

            if (propertiesWithAggregateId.Count == 0)
            {
                throw new InvalidOperationException("Command has no AggregateId");
            }

            if (propertiesWithAggregateId.Count > 1)
            {
                throw new InvalidOperationException("Command has multiple AggregateId");
            }

            var property = propertiesWithAggregateId[0];
            if (property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException("AggregateId type is not a System.String");
            }

            return (string)propertiesWithAggregateId[0].GetValue(command);
        }

        private Func<TCommand, string> BuildPropertyExpression<TCommand>(PropertyInfo property)
        {
            ParameterExpression arg = Expression.Parameter(typeof(TCommand), "command");
            MemberExpression expr = Expression.Property(arg, property);
            Func<TCommand, string> propertyResolver = Expression.Lambda<Func<TCommand, string>>(expr, arg).Compile();
            return propertyResolver;
        }
    }
}
