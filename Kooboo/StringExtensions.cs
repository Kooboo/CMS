using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo
{
    public static class StringExtensions
    {
        public static string Ellipsis(this string str, int length, int ellipsisLength = 3)
        {
            if (str == null)
                return str;

            if (str.Length <= length)
                return str;

            return str.Substring(0, length - ellipsisLength).PadRight(length, '.');
        }

        public static bool EqualsOrNullEmpty(this string str1, string str2, StringComparison comparisonType)
        {
            return String.Compare(str1 ?? "", str2 ?? "", comparisonType) == 0;
        }

        public static string TrimOrNull(this string str)
        {
            if (str == null)
                return str;

            return str.Trim();
        }

        public static string MergeName(this string name)
        {
            return name.Replace(" ", String.Empty);
        }

        public static string SplitName(this string name, bool toLower = true)
        {
            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < name.Length; i++)
            {
                var ch = name[i];
                if (ch >= 'A' && ch <= 'Z' && i > 0)
                {
                    var prev = name[i - 1];
                    if (prev != ' ')
                    {
                        if (prev >= 'A' && prev <= 'Z')
                        {
                            if (i < name.Length - 1)
                            {
                                var next = name[i + 1];
                                if (next >= 'a' && next <= 'z')
                                {
                                    builder.Append(' ');
                                }
                            }
                        }
                        else
                        {
                            builder.Append(' ');
                        }
                    }
                    builder.Append(toLower ? ch.ToString().ToLower() : ch.ToString());
                }
                else
                {
                    builder.Append(ch);
                }
            }
            return builder.ToString();
        }

        public static IEnumerable<string> SplitPattern(this string pattern)
        {
            pattern = pattern.Trim();

            if (!pattern.StartsWith("("))
                return new string[] { pattern };

            var result = new List<string>();
            var layer = 0;
            var builder = new StringBuilder();
            using (var reader = new StringReader(pattern))
            {
                var r = reader.Read();
                while (r != -1)
                {
                    var ch = (char)r;
                    if (ch == '(')
                    {
                        if (layer == 0)
                        {
                            builder.Clear();
                        }
                        layer++;
                    }
                    else if (ch == ')')
                    {
                        layer--;
                        if (layer == 0)
                        {
                            result.Add(builder.ToString());
                        }
                    }
                    else
                    {
                        if (layer > 0)
                        {
                            builder.Append(ch);
                        }
                    }
                    r = reader.Read();
                }
            }
            return result;
        }

        public static string MergePattern(this IEnumerable<string> patterns)
        {
            return String.Join("|", patterns.Select(o => String.Format("({0})", o)));
        }
    }
}
