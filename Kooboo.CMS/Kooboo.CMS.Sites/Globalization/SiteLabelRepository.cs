using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Globalization;
using Kooboo.Globalization.Repository;
using Kooboo.CMS.Sites.Models;

using Kooboo.CMS.Sites.Caching;
using System.Threading;
using System.Collections;
namespace Kooboo.CMS.Sites.Globalization
{
    public class SiteLabelRepository : IElementRepository
    {
        public static string FileCulture = "Labels";
        //ConcurrentDictionary<ElementCacheKey, Element> entries = new ConcurrentDictionary<ElementCacheKey, Element>(new ElementCacheKeyEqualityComparer());

        public SiteLabelRepository(Site site)
        {
            StoreRepository = new XmlElementRepository(new Label(site).PhysicalPath);
        }
        public IElementRepository StoreRepository { get; private set; }

        #region IElementRepository Members

        public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
        {
            return this.StoreRepository.EnabledLanguages();
        }

        public IQueryable<Element> Elements()
        {
            return this.StoreRepository.Elements();
        }

        public Element Get(string name, string category, string culture)
        {
            var label = this.StoreRepository.Get(name, category, FileCulture);
            if (label != null)
            {
                label.Culture = null;
            }
            return label;
        }

        public bool Add(Element element)
        {
            element.Culture = FileCulture;
            return this.StoreRepository.Add(element);
        }

        public bool Update(Element element)
        {
            element.Culture = FileCulture;
            return this.StoreRepository.Update(element);
        }

        public bool Remove(Element element)
        {
            element.Culture = FileCulture;
            return this.StoreRepository.Remove(element);
        }
        #endregion

        #region IElementRepository Members


        public IQueryable<ElementCategory> Categories()
        {
            return StoreRepository.Categories();
        }

        #endregion

        #region IElementRepository Members


        public bool RemoveCategory(string category, string culture)
        {
            return this.StoreRepository.RemoveCategory(category, FileCulture);
        }

        public void AddCategory(string category, string culture)
        {
            this.StoreRepository.AddCategory(category, FileCulture);
        }
        #endregion


        public void Clear()
        {
            StoreRepository.Clear();
        }
    }
}
