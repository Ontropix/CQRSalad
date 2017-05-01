using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace CQRSalad.Infrastructure.ValueInjection
{
    internal static class ValueInjecter
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _cache =
            new ConcurrentDictionary<Type, PropertyInfo[]>();

        private static PropertyInfo[] GetProperties(object obj)
        {
            return _cache.GetOrAdd(obj.GetType(), key => key.GetProperties());
        }
        
        private static bool Match(PropertyInfo one, PropertyInfo two)
        {
            return one.Name == two.Name && one.PropertyType == two.PropertyType;
        }

        public static void Inject(object target, object source)
        {
            PropertyInfo[] sourceProps = GetProperties(source);
            PropertyInfo[] targetProps = GetProperties(target);

            foreach (PropertyInfo sourceProp in sourceProps)
            {
                foreach (PropertyInfo targetProp in targetProps)
                {
                    if (Match(sourceProp, targetProp))
                    {
                        targetProp.SetValue(target, sourceProp.GetValue(source));
                    }
                }
            }
        }
    }
}