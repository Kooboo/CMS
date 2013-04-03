using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Globalization;
using System.Data.Entity.ModelConfiguration.Design.PluralizationServices;

namespace Kooboo.Extensions
{
    public static class StringExtensions
    {
        public static bool IsNullOrEmpty(this string source)
        {

            if (source == null)
            {
                return true;
            }
            else
            {
                return source.Length == 0;
            }
        }

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

        public static bool Contains(this string original, string value, StringComparison comparisionType)
        {
            return original.IndexOf(value, comparisionType) >= 0;
        }


        static Regex invalidUrlCharacter = new Regex(@"[^a-z|^_|^\d|^\u4e00-\u9fa5]+", RegexOptions.Compiled | RegexOptions.Singleline | RegexOptions.IgnoreCase);
        /// <summary>
        /// Replaces to valid URL string.
        /// </summary>
        /// <param name="s">The s.</param>
        /// <returns></returns>
        public static string NormalizeUrl(this string s)
        {
            if (!string.IsNullOrEmpty(s))
            {
                return invalidUrlCharacter.Replace(s.Trim(), "-");
            }
            return s;
        }

        public static string StripHtmlXmlTags(this string content)
        {
            if (string.IsNullOrEmpty(content))
            {
                return content;
            }
            return Regex.Replace(content, "<[^>]+>?", "", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        }

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


        #region Pluralization

        static PluralizationService pluralizationService = PluralizationService.CreateService(CultureInfo.GetCultureInfo("en-us"));

        public static bool IsPlural(this string word)
        {
            return pluralizationService.IsPlural(word);
        }
        public static bool IsSingular(this string word)
        {
            return pluralizationService.IsSingular(word);
        }
        public static string Pluralize(this string word)
        {
            return pluralizationService.Pluralize(word);
        }
        public static string Singularize(this string word)
        {
            return pluralizationService.Singularize(word);
        }


        #endregion
    }

}
