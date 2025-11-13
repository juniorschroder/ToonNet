using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ToonNet.Serialization
{
    public static class ToonSerializer
    {
        public static string Serialize<T>(IEnumerable<T> objects, string rootName)
        {
            if (objects == null) throw new ArgumentNullException(nameof(objects));
            var list = objects.ToList();
            if (!list.Any()) return $"{rootName}[0]{{}}:";

            var type = typeof(T);
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(p => p.CanRead)
                .ToArray();

            var sb = new StringBuilder();
            sb.Append($"{rootName}[{list.Count}]{{");
            sb.Append(string.Join(",", props.Select(p => p.Name)));
            sb.AppendLine("}:");

            foreach (var obj in list)
            {
                var values = props.Select(p =>
                {
                    var val = p.GetValue(obj);
                    return val == null ? "" : val.ToString().Replace(",", "\\,");
                });
                sb.AppendLine($"  {string.Join(",", values)}");
            }

            return sb.ToString().TrimEnd();
        }
    }
}