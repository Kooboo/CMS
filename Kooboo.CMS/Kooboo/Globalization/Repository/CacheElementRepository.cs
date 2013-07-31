#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Linq;

namespace Kooboo.Globalization.Repository
{
    /// <summary>
    /// 
    /// </summary>
    public class CacheElementRepository : IElementRepository
    {
        #region Fields
        System.Collections.Hashtable CachedElements = new System.Collections.Hashtable(new ElementCacheKeyEqualityComparer());
        private Func<IElementRepository> inner;
        #endregion

        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="CacheElementRepository" /> class.
        /// </summary>
        /// <param name="innerRepository">The inner repository.</param>
        public CacheElementRepository(Func<IElementRepository> innerRepository)
        {
            inner = innerRepository;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Enableds the languages.
        /// </summary>
        /// <returns></returns>
        public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
        {
            return inner().EnabledLanguages();
        }

        /// <summary>
        /// Elementses this instance.
        /// </summary>
        /// <returns></returns>
        public IQueryable<Element> Elements()
        {
            return inner().Elements();
        }

        /// <summary>
        /// Gets the specified name.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public Element Get(string name, string category, string culture)
        {
            var cachedKey = new ElementCacheKey(name, category, culture);
            Element element = CachedElements[cachedKey] as Element;

            //.FirstOrDefault(i => i.Name == key && i.Category == category && i.Culture == culture.Name);
            if (element == null)
            {
                element = inner().Get(name, category, culture);

                if (element != null)
                {
                    AddCache(element);
                }
            }
            return element;
        }

        /// <summary>
        /// Adds the cache.
        /// </summary>
        /// <param name="element">The element.</param>
        public void AddCache(Element element)
        {
            var cacheKey = new ElementCacheKey(element);
            CachedElements[cacheKey] = element;

        }
        /// <summary>
        /// Removes the cache.
        /// </summary>
        /// <param name="element">The element.</param>
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
        /// <summary>
        /// Clears the cache.
        /// </summary>
        public void ClearCache()
        {
            lock (CachedElements)
            {
                CachedElements.Clear();
            }
        }
        /// <summary>
        /// Categorieses this instance.
        /// </summary>
        /// <returns></returns>
        public IQueryable<ElementCategory> Categories()
        {
            return inner().Categories();
        }

        /// <summary>
        /// Adds the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public bool Add(Element element)
        {
            var r = inner().Add(element);
            if (r)
            {
                AddCache(element);
            }
            return r;
        }

        /// <summary>
        /// Updates the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public bool Update(Element element)
        {
            var r = inner().Update(element);
            if (r)
            {
                RemoveCache(element);
            }
            return r;
        }

        /// <summary>
        /// Removes the specified element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <returns></returns>
        public bool Remove(Element element)
        {
            var r = inner().Remove(element);
            if (r)
            {
                RemoveCache(element);
            }
            return r;
        }

        /// <summary>
        /// Adds the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        public void AddCategory(string category, string culture)
        {
            inner().AddCategory(category, culture);
        }

        /// <summary>
        /// Removes the category.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <returns></returns>
        public bool RemoveCategory(string category, string culture)
        {
            var r = inner().RemoveCategory(category, culture);
            ClearCache();
            return r;
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            inner().Clear();
            CachedElements.Clear();
        }
        #endregion
    }
}
