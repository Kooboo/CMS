#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration.Design.PluralizationServices;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Kooboo
{
    /// <summary>
    /// 
    /// </summary>
    public static class StringExtensions
    {
        #region Methods
        /// <summary>
        /// Ellipsises the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="length">The length.</param>
        /// <param name="ellipsisLength">Length of the ellipsis.</param>
        /// <returns></returns>
        public static string Ellipsis(this string str, int length, int ellipsisLength = 3)
        {
            if (str == null)
                return str;

            if (str.Length <= length)
                return str;

            return str.Substring(0, length - ellipsisLength).PadRight(length, '.');
        }

        /// <summary>
        /// Equalses the or null empty.
        /// </summary>
        /// <param name="str1">The STR1.</param>
        /// <param name="str2">The STR2.</param>
        /// <param name="comparisonType">Type of the comparison.</param>
        /// <returns></returns>
        public static bool EqualsOrNullEmpty(this string str1, string str2, StringComparison comparisonType)
        {
            return String.Compare(str1 ?? "", str2 ?? "", comparisonType) == 0;
        }

        /// <summary>
        /// Trims the or null.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns></returns>
        public static string TrimOrNull(this string str)
        {
            if (str == null)
                return str;

            return str.Trim();
        }

        /// <summary>
        /// Merges the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns></returns>
        public static string MergeName(this string name)
        {
            return name.Replace(" ", String.Empty);
        }

        /// <summary>
        /// Splits the name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="toLower">if set to <c>true</c> [to lower].</param>
        /// <returns></returns>
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
        /// <summary>
        /// Ases the specified source.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="source">The source.</param>
        /// <returns></returns>
        public static T As<T>(this string source)
        {
            if (source == null)
            {
                return default(T);
            }

            try
            {
                return (T)Convert.ChangeType(source, typeof(T));
            }
            catch
            {
                return default(T);
            }
        }

        public static T As<T>(this string source, T defaultValue)
        {
            if (source == null)
            {
                return defaultValue;
            }

            try
            {
                return (T)Convert.ChangeType(source, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Ases the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static object As(this string source, Type type)
        {
            if (source == null)
            {
                return null;
            }

            try
            {
                return Convert.ChangeType(source, type);
            }
            catch
            {
                return null;
            }
        }

        /// <summary>
        /// Itemses the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="itemIndex">Index of the item.</param>
        /// <param name="separator">The separator.</param>
        /// <returns></returns>
        public static string Items(this string source, int itemIndex, string separator = ",")
        {

            if (source == null)
            {
                return string.Empty;
            }
            else
            {
                var items = source.Split(new string[] { separator }, StringSplitOptions.RemoveEmptyEntries);
                if (items.Length > itemIndex)
                {
                    return items[itemIndex];
                }
                else
                {
                    return string.Empty;
                }

            }
        }

        /// <summary>
        /// Determines whether [contains] [the specified original].
        /// </summary>
        /// <param name="original">The original.</param>
        /// <param name="value">The value.</param>
        /// <param name="comparisionType">Type of the comparision.</param>
        /// <returns>
        ///   <c>true</c> if [contains] [the specified original]; otherwise, <c>false</c>.
        /// </returns>
        public static bool Contains(this string original, string value, StringComparison comparisionType)
        {
            return original.IndexOf(value, comparisionType) >= 0;
        }

        /// <summary>
        /// Strips the HTML XML tags.
        /// </summary>
        /// <param name="content">The content.</param>
        /// <returns></returns>
        public static string StripHtmlXmlTags(this string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }
            return Regex.Replace(content, "<[^>]+>?", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

        /// <summary>
        /// Strips all tags.
        /// </summary>
        /// <param name="stringToStrip">The string to strip.</param>
        /// <returns></returns>
        public static string StripAllTags(this string stringToStrip)
        {
            if (string.IsNullOrEmpty(stringToStrip))
            {
                return stringToStrip;
            }
            // paring using RegEx
            //
            stringToStrip = Regex.Replace(stringToStrip, "</p(?:\\s*)>(?:\\s*)<p(?:\\s*)>", "\n\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            stringToStrip = Regex.Replace(stringToStrip, "<br(?:\\s*)/>", "\n", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            stringToStrip = Regex.Replace(stringToStrip, "\"", "''", RegexOptions.IgnoreCase | RegexOptions.Compiled);
            stringToStrip = StripHtmlXmlTags(stringToStrip);

            return stringToStrip;
        }
        #endregion


        #region Pluralization

        static PluralizationService pluralizationService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));

        /// <summary>
        /// Determines whether the specified word is plural.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>
        ///   <c>true</c> if the specified word is plural; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsPlural(this string word)
        {
            return pluralizationService.IsPlural(word);
        }
        /// <summary>
        /// Determines whether the specified word is singular.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns>
        ///   <c>true</c> if the specified word is singular; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsSingular(this string word)
        {
            return pluralizationService.IsSingular(word);
        }
        /// <summary>
        /// Pluralizes the specified word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public static string Pluralize(this string word)
        {
            return pluralizationService.Pluralize(word);
        }
        /// <summary>
        /// Singularizes the specified word.
        /// </summary>
        /// <param name="word">The word.</param>
        /// <returns></returns>
        public static string Singularize(this string word)
        {
            return pluralizationService.Singularize(word);
        }


        #endregion
    }
}
