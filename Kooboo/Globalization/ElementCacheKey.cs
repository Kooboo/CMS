using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace Kooboo.Globalization
{
    public class ElementCacheKeyEqualityComparer : IEqualityComparer
    {

        #region IEqualityComparer<ElementCacheKey> Members

        public bool Equals(object x, object y)
        {
            return x.Equals(y);
        }

        public int GetHashCode(object obj)
        {
            ElementCacheKey key = (ElementCacheKey)obj;
            return key.Hash;
        }

        #endregion
    }



    public class ElementCacheKey
    {
        public ElementCacheKey(Element element)
            : this(element.Name, element.Category, element.Culture)
        {

        }
        public ElementCacheKey(string name, string category, string culture)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException("name");
            this.Name = name ?? "";
            this.Category = category ?? "";
            this.Culture = culture ?? "";

            this.Hash = string.Format("name:{0};category:{1};culture:{2}",
                this.Name.ToLower(),
                this.Category,
                this.Culture.ToLower()).GetHashCode();
        }
        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Culture { get; private set; }

        public int Hash { get; private set; }

        public override int GetHashCode()
        {
            return Hash;
        }
        public override bool Equals(object obj)
        {
            return this.Hash == ((ElementCacheKey)obj).GetHashCode();
        }
    }
}
