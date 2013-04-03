using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo.Globalization
{
    public class LocalizeHelper
    {
        public const string StringPattern = @"([^""]|\\""|"""")*";
        public static string LocalizePattern = String.Format(@"(?<![""\\])""(?<s>{0})"".Localize\(\)", StringPattern);
        public static string ErrorMessagePattern = String.Format(@"ErrorMessage\s*=\s*""(?<s>{0})""", StringPattern);
        public static string DisplayNamePattern = String.Format(@"DisplayName\(\s*""(?<s>{0})""\s*\)", StringPattern);
        public static string DescriptionPattern = String.Format(@"Description\(\s*""(?<s>{0})""\s*\)", StringPattern);
        public static string DisplayPattern = String.Format(@"Display\(.*\bName\s*=\s*""(?<s>{0})"".*\)", StringPattern);

        public static void GenerateResource(string dir, params string[] patterns)
        {
            var ps = patterns.Union(new string[]
            {
                LocalizePattern,
                ErrorMessagePattern,
                DisplayNamePattern,
                DescriptionPattern,
                DisplayPattern
            });
            foreach (var file in GetFiles(dir, "*.cs", "*.ascx", "*.aspx", "*.cshtml"))
            {
                var content = File.ReadAllText(file);
                foreach (var pattern in ps)
                {
                    foreach (Match match in Regex.Matches(content, pattern))
                    {
                        match.Groups["s"].Value.Localize();
                    }
                }
            }
        }

        private static IEnumerable<string> GetFiles(string dir, params string[] extensions)
        {
            var list = new List<string>();
            foreach (var each in extensions)
            {
                list.AddRange(Directory.GetFiles(dir, each, SearchOption.AllDirectories));
            }
            return list;
        }
    }
}
