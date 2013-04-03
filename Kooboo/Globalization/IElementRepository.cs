using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Kooboo.Globalization
{
    public class ElementCategory
    {
        public string Category { get; set; }
        public string Culture { get; set; }
    }
    public interface IElementRepository
    {
        IQueryable<CultureInfo> EnabledLanguages();

        IQueryable<Element> Elements();

        Element Get(string name, string category, string culture);

        IQueryable<ElementCategory> Categories();

        bool Add(Element element);

        bool Update(Element element);

        bool Remove(Element element);

        void Clear();

        void AddCategory(string category, string culture);

        bool RemoveCategory(string category, string culture);
    }
}
