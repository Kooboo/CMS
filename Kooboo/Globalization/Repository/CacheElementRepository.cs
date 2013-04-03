using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Globalization.Repository
{
    public class CacheElementRepository : IElementRepository
    {
        System.Collections.Hashtable CachedElements = new System.Collections.Hashtable(new ElementCacheKeyEqualityComparer());
        private IElementRepository inner;
        public CacheElementRepository(IElementRepository innerRepository)
        {
            inner = innerRepository;
        }
        public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
        {
            return inner.EnabledLanguages();
        }

        public IQueryable<Element> Elements()
        {
            return inner.Elements();
        }

        public Element Get(string name, string category, string culture)
        {
            var cachedKey = new ElementCacheKey(name, category, culture);
            Element element = CachedElements[cachedKey] as Element;

            //.FirstOrDefault(i => i.Name == key && i.Category == category && i.Culture == culture.Name);
            if (element == null)
            {
                element = inner.Get(name, category, culture);

                if (element != null)
                {
                    AddCache(element);
                }
            }
            return element;
        }

        public void AddCache(Element element)
        {
            var cacheKey = new ElementCacheKey(element);
            CachedElements[cacheKey] = element;

        }
        public void RemoveCache(Element element)
        {
            var cacheKey = new ElementCacheKey(element);
            lock (CachedElements)
            {
                if (CachedElements.ContainsKey(cacheKey))
                {
                    CachedElements.Remove(cacheKey);
                }
            }
        }
        public void ClearCache()
        {
            lock (CachedElements)
            {
                CachedElements.Clear();
            }
        }
        public IQueryable<ElementCategory> Categories()
        {
            return inner.Categories();
        }

        public bool Add(Element element)
        {
            var r = inner.Add(element);
            if (r)
            {
                AddCache(element);
            }
            return r;
        }

        public bool Update(Element element)
        {
            var r = inner.Update(element);
            if (r)
            {
                RemoveCache(element);
            }
            return r;
        }

        public bool Remove(Element element)
        {
            var r = inner.Remove(element);
            if (r)
            {
                RemoveCache(element);
            }
            return r;
        }

        public void AddCategory(string category, string culture)
        {
            inner.AddCategory(category, culture);
        }

        public bool RemoveCategory(string category, string culture)
        {
            var r = inner.RemoveCategory(category, culture);
            ClearCache();
            return r;
        }

        public void Clear()
        {
            inner.Clear();
            CachedElements.Clear();
        }
    }
}
