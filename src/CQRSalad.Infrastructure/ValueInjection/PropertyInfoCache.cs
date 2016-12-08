using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace CQRSalad.EventSourcing.ValueInjection
{
    internal static class PropertyInfoCache
    {
        private static readonly ConcurrentDictionary<Type, PropertyInfo[]> _cache = new ConcurrentDictionary<Type, PropertyInfo[]>();

        public static PropertyInfo[] GetProperties(Type type)
        {
            if (_cache.ContainsKey(type))
            {
                return _cache[type];
            }
            
            PropertyInfo[] props = type.GetProperties();
            _cache.TryAdd(type, props);

            return _cache[type];
        }

        public static PropertyInfo[] GetProperties(object obj)
        {
            return GetProperties(obj.GetType());
        }
    }
}
