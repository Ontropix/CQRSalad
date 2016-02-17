using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace CQRSalad.Dispatching
{
    internal static class ReflectionExtensions
    {
        internal static TAttribute FirstOrDefaultAttribute<TAttribute>(this MemberInfo type, bool lookInherited = false) where TAttribute : Attribute
        {
            object[] attributes = type.GetCustomAttributes(typeof(TAttribute), lookInherited);

            if (attributes.Length > 0)
            {
                return (TAttribute)attributes[0];
            }

            return default(TAttribute);
        }
        
        internal static bool IsClonable(this Type type)
        {
            var memberTypes = new List<Type>();
            const BindingFlags staticFlags = BindingFlags.Static | BindingFlags.NonPublic | BindingFlags.Public;
            const BindingFlags instanceFlags = BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

            //adding static fields and properties
            memberTypes.AddRange(type.GetFields(staticFlags).Select(x => x.FieldType));
            memberTypes.AddRange(type.GetProperties(staticFlags).Select(x => x.PropertyType));

            //adding instance fields and properties
            memberTypes.AddRange(type.GetFields(instanceFlags).Select(x => x.FieldType));
            memberTypes.AddRange(type.GetProperties(instanceFlags).Select(x => x.PropertyType));

            bool result = memberTypes.All(memberType =>
            {
                if (memberType.IsPrimitive || memberType == typeof(String) || memberType == typeof(DateTime) || memberType.IsEnum)
                {
                    return true;
                }

                //if it is not primitive, not enum, but still value type -- it is structure
                if (memberType.IsValueType)
                {
                    //we check all structure fields and properties
                    return IsClonable(memberType);
                }

                //any other variants
                return false;
            });

            return result;
        }
    }
}