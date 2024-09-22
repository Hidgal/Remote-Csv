using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace RemoteCsv.Internal.Extensions
{
    public static class FieldAttributeExtension
    {
        public static FromCsvAttribute GetCsvAttribute(this FieldInfo info) => info.GetCustomAttribute<FromCsvAttribute>();
        public static IEnumerable<FieldInfo> GetFieldsWithCsvAttribute(this Type targetType) => GetFieldsWithAttribute<FromCsvAttribute>(targetType);
        
        public static FieldInfo GetInstanceField(this Type type, string fieldName)
        {
            return type.GetField(fieldName, BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }
        public static FieldInfo[] GetInstanceFields(this Type type)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
        }

        public static IEnumerable<FieldInfo> GetFieldsWithAttribute(this Type targetType, Type attributeType)
        {
            return targetType
                .GetInstanceFields()
                .Where(field => field.GetCustomAttributes(attributeType).Count() > 0);
        }
        public static IEnumerable<FieldInfo> GetFieldsWithAttribute<TAttribute>(this Type targetType) where TAttribute : Attribute
        {
            return targetType
                .GetInstanceFields()
                .Where(field => field.GetCustomAttributes<TAttribute>().Count() > 0);
        }
    }
}

