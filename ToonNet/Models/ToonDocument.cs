using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ToonNet.Models
{
    public class ToonDocument
    {
        public string RootName { get; set; } = string.Empty;
        public List<string> Fields { get; set; } = new List<string>();
        public List<string[]> Rows { get; set; } = new List<string[]>();

        public int Count => Rows.Count;

        public override string ToString()
        {
            var header = $"{RootName}[{Count}]{{{string.Join(",", Fields)}}}:";
            var lines = Rows.Select(r => "  " + string.Join(",", r));
            return string.Join(Environment.NewLine, new[] { header }.Concat(lines));
        }
        
        public static ToonDocument Parse(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
                throw new ArgumentException("Input text cannot be null or empty.", nameof(text));

            var document = new ToonDocument();

            // Regex para capturar cabeçalho: Ex: users[2]{Id,Name,Role}:
            var headerMatch = Regex.Match(text, @"^(\w+)\[(\d+)\]\{([^}]+)\}:", RegexOptions.Multiline);
            if (!headerMatch.Success)
                throw new FormatException("Invalid TOON header format.");

            document.RootName = headerMatch.Groups[1].Value.Trim();
            var count = int.Parse(headerMatch.Groups[2].Value);
            document.Fields = headerMatch.Groups[3].Value.Split(',').Select(f => f.Trim()).ToList();

            // Extrair linhas após o cabeçalho
            var lines = text.Split('\n')
                .SkipWhile(l => !l.Contains(':'))
                .Skip(1)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .Select(l => l.Trim())
                .ToList();

            foreach (var line in lines)
            {
                var values = line.Split(',').Select(v => v.Trim()).ToArray();
                if (values.Length != document.Fields.Count)
                    throw new FormatException($"Row '{line}' does not match field count ({document.Fields.Count}).");
                document.Rows.Add(values);
            }

            // Valida quantidade
            if (document.Rows.Count != count)
                throw new FormatException($"Declared count {count} does not match actual rows ({document.Rows.Count}).");

            return document;
        }
    }
}