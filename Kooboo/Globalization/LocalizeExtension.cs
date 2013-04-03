using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using Kooboo;
using System.Diagnostics.Contracts;

namespace Kooboo.Globalization
{
    public static class LocalizeExtension
    {
        public static string Localize(this string source, CultureInfo culture)
        {
            return source.Localize(null, culture);

        }

        public static string Localize(this string source, string category = "")
        {
            return source.Localize(category, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

        public static string Localize(this string source, string category, CultureInfo culture)
        {
            return source.Map(source, category, culture).Value;
        }

        public static Element Map(this string source, string key, string category = "")
        {
            return source.Map(key, category, System.Threading.Thread.CurrentThread.CurrentUICulture);
        }

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

        public static string Localize(this Element element)
        {
            return element.Value
                .Map(element.Name, element.Category, CultureInfo.GetCultureInfo(element.Culture))
                .Value;
        }

        public static string Position(this string str, string position)
        {
            return String.Format("<span title=\"{1}\">{0}</span>", str, position);
        }
    }
}
