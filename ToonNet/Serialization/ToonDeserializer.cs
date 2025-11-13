using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace ToonNet.Serialization
{
    public static class ToonDeserializer
    {
        public static List<T> Deserialize<T>(string toon) where T : new()
        {
            if (string.IsNullOrWhiteSpace(toon))
                throw new ArgumentException("TOON input is empty.", nameof(toon));

            var lines = toon.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);
            if (lines.Length < 2) throw new FormatException("Invalid TOON format.");

            var header = lines[0];
            var fieldsStart = header.IndexOf('{') + 1;
            var fieldsEnd = header.IndexOf('}');
            var fieldNames = header.Substring(fieldsStart, fieldsEnd - fieldsStart)
                                   .Split(',')
                                   .Select(f => f.Trim())
                                   .ToArray();

            var props = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                 .Where(p => p.CanWrite)
                                 .ToDictionary(p => p.Name, StringComparer.OrdinalIgnoreCase);

            var list = new List<T>();

            foreach (var line in lines.Skip(1))
            {
                var trimmed = line.Trim();
                if (string.IsNullOrEmpty(trimmed)) continue;

                var parts = SplitEscaped(trimmed);
                var obj = new T();

                for (int i = 0; i < fieldNames.Length && i < parts.Length; i++)
                {
                    var name = fieldNames[i];
                    if (!props.TryGetValue(name, out var prop)) continue;

                    var value = parts[i].Replace("\\,", ",");
                    object converted = Convert.ChangeType(value, prop.PropertyType);
                    prop.SetValue(obj, converted);
                }

                list.Add(obj);
            }

            return list;
        }

        private static string[] SplitEscaped(string input)
        {
            var result = new List<string>();
            var current = "";
            bool escaped = false;

            foreach (var c in input)
            {
                if (escaped)
                {
                    current += c;
                    escaped = false;
                }
                else if (c == '\\')
                {
                    escaped = true;
                }
                else if (c == ',')
                {
                    result.Add(current);
                    current = "";
                }
                else
                {
                    current += c;
                }
            }

            result.Add(current);
            return result.ToArray();
        }
    }
}
