using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.Globalization.Repository
{
    public class MemoryElementRepository:IElementRepository
    {
        static Dictionary<string, Element> CachedElements
        {
            get;
            set;
        }

        public MemoryElementRepository(IElementRepository elementRepository)
        {
            if (elementRepository != null)
            {
                this.InnerElementRepository = elementRepository;
                this.InnerElementRepository.Added += new Action<Element>(InnerElementRepository_Added);
                this.InnerElementRepository.Removed += new Action<Element>(InnerElementRepository_Removed);
                this.InnerElementRepository.Updated += new Action<Element>(InnerElementRepository_Updated);
            }
            CachedElements = new Dictionary<string, Element>();
        }

        void InnerElementRepository_Updated(Element obj)
        {
            lock (CachedElements)
            {
                CachedElements[GetElementDictionaryKey(obj)] = obj;
            }
        }

        void InnerElementRepository_Removed(Element obj)
        {
            lock (CachedElements)
            {
                CachedElements.Remove(GetElementDictionaryKey(obj));
            }
        }

        void InnerElementRepository_Added(Element obj)
        {
            lock (CachedElements)
            {
                CachedElements[GetElementDictionaryKey(obj)] = obj;
            }
        }

        IElementRepository InnerElementRepository
        {
            get;
            set;
        }

        public IQueryable<System.Globalization.CultureInfo> EnabledLanguages()
        {
            if (this.InnerElementRepository != null)
            {
                return this.InnerElementRepository.EnabledLanguages();
            }

            return Enumerable.Empty<System.Globalization.CultureInfo>().AsQueryable();
        }

        public IQueryable<Element> Elements()
        {
            if (this.InnerElementRepository != null)
            {
                return this.InnerElementRepository.Elements();
            }

            return CachedElements.Values.AsQueryable();
        }

        public Element Get(string name, string category, string culture)
        {
            string dictionaryKey = GetElementDictionaryKey(name, category, culture);

            Element element = null;
            if (CachedElements.ContainsKey(dictionaryKey))
            {
                element = CachedElements[dictionaryKey];
            }

            if (element == null && this.InnerElementRepository != null)
            {
                element = this.InnerElementRepository.Get(name, category, culture); 

                if (element != null)
                {
                    CachedElements.Add(dictionaryKey, element);
                }
            }

            return element;
        }

        public bool Add(Element element)
        {
            var result = false;

            if (this.InnerElementRepository != null)
            {
                result = this.InnerElementRepository.Add(element);
                if (result == true)
                {
                    this.OnAdded(element);
                }
            }
            else
            {
                InnerElementRepository_Added(element);
                this.OnAdded(element);
                return true;
            }

            return result;
        }

        public bool Update(Element element)
        {
            var result = false;

            if (this.InnerElementRepository != null)
            {
                this.InnerElementRepository.Update(element);

                if (result == true)
                {
                    this.OnUpdated(element);
                }
            }
            else
            {
                InnerElementRepository_Updated(element);
                this.OnAdded(element);
                return true;
            }

            return result;
        }

        public bool Remove(Element element)
        {
            var result = false;

            if (this.InnerElementRepository != null)
            {
                this.InnerElementRepository.Remove(element);

                if (result == true)
                {
                    this.OnRemoved(element);
                }
            }
            else
            {
                InnerElementRepository_Removed(element);
                this.OnRemoved(element);
                return true;
            }

            return result;
        }

        void OnAdded(Element element)
        {
            var added = this.Added;

            if (added != null)
            {
                added(element);
            }
        }

        void OnRemoved(Element element)
        {
            var removed = this.Removed;

            if (removed != null)
            {
                removed(element);
            }
        }

        void OnUpdated(Element element)
        {
            var updated = this.Updated;

            if (updated != null)
            {
                updated(element);
            }
        }

        string GetElementDictionaryKey(Element element)
        {
            return GetElementDictionaryKey(element.Name, element.Category, element.Culture);
        }

        string GetElementDictionaryKey(string name, string category, string culture)
        {
            return name + category + culture;
        }
        
        public event Action<Element> Added;

        public event Action<Element> Updated;

        public event Action<Element> Removed;
    }
}
