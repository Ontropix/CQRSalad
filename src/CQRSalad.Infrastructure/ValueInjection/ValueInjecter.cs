using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CQRSalad.Infrastructure.ValueInjection
{
    internal static class ValueInjecter
    {
        private static readonly ConcurrentDictionary<Type, List<PropertyInfo>> _cache =
            new ConcurrentDictionary<Type, List<PropertyInfo>>();

        private static List<PropertyInfo> GetProperties(object obj)
        {
            return _cache.GetOrAdd(obj.GetType(), key => key.GetProperties().OrderBy(x => x.Name).ToList());
        }
        
        private static bool MatchProps(PropertyInfo one, PropertyInfo two)
        {
            return one.Name == two.Name && one.PropertyType == two.PropertyType;
        }

        // TODO bad performance. Need to replace it with Delegates/Reflection Emit
        public static void Inject(object source, object target)
        {
            var sourceProps = GetProperties(source);
            var targetProps = GetProperties(target);

            foreach (var sourceProp in sourceProps)
            {
                foreach (var targetProp in targetProps)
                {
                    if (MatchProps(sourceProp, targetProp))
                    {
                        targetProp.SetValue(target, sourceProp.GetValue(source));
                        break;
                    }
                }
            }
        }
    }
}