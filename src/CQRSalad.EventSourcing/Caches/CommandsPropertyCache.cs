using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace CQRSalad.EventSourcing
{
    internal static class CommandsPropertyCache
    {
        // CommandType - Property getter delegate
        private static readonly ConcurrentDictionary<Type, Func<object, string>> _cache =
            new ConcurrentDictionary<Type, Func<object, string>>();

        internal static Func<object, string> GetAggregateIdProp(Type commandType)
        {
            return _cache.GetOrAdd(commandType, key =>
            {
                PropertyInfo property = ResolveProperty(commandType);
                return BuildPropertyResolver(commandType, property);
            });
        }

        private static PropertyInfo ResolveProperty(Type targetType)
        {
            List<PropertyInfo> propertiesWithAggregateId =
                targetType
                    .GetProperties(BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
                    .Where(prop => prop.IsDefined(typeof(AggregateIdAttribute), false))
                    .ToList();

            if (propertiesWithAggregateId.Count == 0)
            {
                throw new InvalidOperationException("Command has no AggregateId.");
            }

            if (propertiesWithAggregateId.Count > 1)
            {
                throw new InvalidOperationException("Command has multiple AggregateId.");
            }

            var property = propertiesWithAggregateId[0];
            if (property.PropertyType != typeof(string))
            {
                throw new InvalidOperationException("AggregateId is not a System.String.");
            }

            return property;
        }

        private static Func<object, string> BuildPropertyResolver(Type targetType, PropertyInfo property)
        {
            ParameterExpression parameter = Expression.Parameter(typeof(object), "target");
            UnaryExpression converted = Expression.Convert(parameter, targetType);
            MemberExpression expr = Expression.Property(converted, property);
            return Expression.Lambda<Func<object, string>>(expr, parameter).Compile();
        }
    }
}