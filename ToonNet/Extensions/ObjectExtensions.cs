using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ToonNet.Extensions
{
    public static class ObjectExtensions
    {
        public static Dictionary<string, object> GetReadableProperties(this object obj)
        {
            if (obj == null) throw new ArgumentNullException(nameof(obj));

            return obj.GetType()
                .GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .ToDictionary(p => p.Name, p => p.GetValue(obj, null));
        }

        public static string EscapeToonValue(this object value)
        {
            if (value == null) return string.Empty;
            var str = value.ToString();
            return str?.Replace(",", "\\,") ?? "";
        }

        public static object ConvertTo(this string str, Type targetType)
        {
            if (string.IsNullOrEmpty(str))
                return targetType.IsValueType ? Activator.CreateInstance(targetType) : null;

            return Convert.ChangeType(str, targetType);
        }
    }
}