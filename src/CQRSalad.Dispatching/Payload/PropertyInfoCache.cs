using System;
using System.Collections.Concurrent;
using System.Reflection;

namespace CQRSalad.Dispatching.Payload
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

            return props;
        }

        public static PropertyInfo[] GetProperties(object obj)
        {
            return GetProperties(obj.GetType());
        }
        
        private static object CloneMessage(object message)
        {
            PropertyInfo[] properties = GetProperties(message);
            object messageCopy = Activator.CreateInstance(message.GetType());

            foreach (PropertyInfo prop in properties)
            {
                object propValue = prop.GetValue(message);
                prop.SetValue(messageCopy, propValue);
            }

            return messageCopy;
        }
    }
}
