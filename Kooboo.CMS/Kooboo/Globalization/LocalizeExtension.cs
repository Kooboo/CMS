#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Globalization;

namespace Kooboo.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public static class LocalizeExtension
    {
        #region Methods
        /// <summary>
        /// Localizes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static string Localize(this string source, CultureInfo culture)
        {
            return source.Localize(null, culture);

        }

        /// <summary>
        /// Localizes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public static string Localize(this string source, string category = "")
        {
            return source.Localize(category, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// Localizes the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static string Localize(this string source, string category, CultureInfo culture)
        {
            return source.Map(source, category, culture).Value;
        }

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="category">The category.</param>
        /// <returns></returns>
        public static Element Map(this string source, string key, string category = "")
        {
            return source.Map(key, category, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        /// <summary>
        /// Maps the specified source.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="key">The key.</param>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public static Element Map(this string source, string key, string category, CultureInfo culture)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                return Element.Empty;
            }

            var repository = ElementRepository.DefaultRepository;

            var element = repository.Get(key, category, culture.Name);
            if (element == null)
            {
                element = new Element() { Name = source, Category = category, Culture = culture.Name, Value = source };

                repository.Add(element);
            }
            return element;
        }

        /// <summary>
        /// Localizes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public static string Localize(this Element element)
        {
            return element.Value
                .Map(element.Name, element.Category, CultureInfo.GetCultureInfo(element.Culture))
                .Value;
        }

        /// <summary>
        /// Positions the specified STR.
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <param name="position">The position.</param>
        /// <returns></returns>
        public static string Position(this string str, string position)
        {
            return String.Format("<span title=\"{1}\">{0}</span>", str, position);
        }
        #endregion
    }
}
