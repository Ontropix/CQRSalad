using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading;

namespace CQRSalad.EventSourcing
{
    public static class StaticAccessors
    {
        private static Dictionary<string, Func<object, object>> getterFnCache = new Dictionary<string, Func<object, object>>();
        private static Dictionary<string, Action<object, object>> setterFnCache = new Dictionary<string, Action<object, object>>();

        public static Func<object, object> GetFastGetter(this Type type, string propName)
        {
            string key = $"{type.FullName}::{propName}";
            Func<object, object> fn;
            if (getterFnCache.TryGetValue(key, out fn))
            {
                return fn;
            }

            PropertyInfo property = type.GetProperty(propName);
            if (property == null)
            {
                return null;
            }

            fn = GetValueGetter(property, type);

            Dictionary<string, Func<object, object>> snapshot, newCache;
            do
            {
                snapshot = getterFnCache;
                newCache = new Dictionary<string, Func<object, object>>(getterFnCache) { [key] = fn };

            } while (!ReferenceEquals(Interlocked.CompareExchange(ref getterFnCache, newCache, snapshot), snapshot));

            return fn;
        }

        public static Action<object, object> GetFastSetter(this Type type, string propName)
        {
            string key = $"{type.FullName}::{propName}";
            Action<object, object> fn;
            if (setterFnCache.TryGetValue(key, out fn))
            {
                return fn;
            }

            var property = type.GetProperty(propName);
            if (property == null)
            {
                return null;
            }

            fn = GetValueSetter(property, type);

            Dictionary<string, Action<object, object>> snapshot, newCache;
            do
            {
                snapshot = setterFnCache;
                newCache = new Dictionary<string, Action<object, object>>(setterFnCache) { [key] = fn };

            } while (!ReferenceEquals(
                Interlocked.CompareExchange(ref setterFnCache, newCache, snapshot), snapshot));

            return fn;
        }

        public static Func<object, object> GetValueGetter(this PropertyInfo propertyInfo, Type type)
        {
            var instance = Expression.Parameter(typeof(object), "i");
            var convertInstance = Expression.TypeAs(instance, type);
            var property = Expression.Property(convertInstance, propertyInfo);
            var convertProperty = Expression.TypeAs(property, typeof(object));
            return Expression.Lambda<Func<object, object>>(convertProperty, instance).Compile();
        }

        public static Func<T, object> GetValueGetter<T>(this PropertyInfo propertyInfo)
        {
            var instance = Expression.Parameter(typeof(T), "i");
            var property = typeof(T) != propertyInfo.DeclaringType
                ? Expression.Property(Expression.TypeAs(instance, propertyInfo.DeclaringType), propertyInfo)
                : Expression.Property(instance, propertyInfo);
            var convertProperty = Expression.TypeAs(property, typeof(object));
            return Expression.Lambda<Func<T, object>>(convertProperty, instance).Compile();
        }

        public static Action<object, object> GetValueSetter(this PropertyInfo propertyInfo, Type instanceType)
        {
            var instance = Expression.Parameter(typeof(object), "i");
            var argument = Expression.Parameter(typeof(object), "a");

            var type = (Expression)Expression.TypeAs(instance, instanceType);

            var setterCall = Expression.Call(
                type,
                propertyInfo.SetMethod,
                Expression.Convert(argument, propertyInfo.PropertyType));

            return Expression.Lambda<Action<object, object>>
            (
                setterCall, instance, argument
            ).Compile();
        }
    }
}