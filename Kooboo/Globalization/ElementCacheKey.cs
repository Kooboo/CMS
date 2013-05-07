#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections;

namespace Kooboo.Globalization
{
    /// <summary>
    /// 
    /// </summary>
    public class ElementCacheKeyEqualityComparer : IEqualityComparer
    {

        #region IEqualityComparer<ElementCacheKey> Members

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="x">The <see cref="System.Object" /> to compare with this instance.</param>
        /// <param name="y">The y.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public new bool Equals(object x, object y)
        {
            return x.Equals(y);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public int GetHashCode(object obj)
        {
            ElementCacheKey key = (ElementCacheKey)obj;
            return key.Hash;
        }

        #endregion
    }



    /// <summary>
    /// 
    /// </summary>
    public class ElementCacheKey
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementCacheKey" /> class.
        /// </summary>
        /// <param name="element">The element.</param>
        public ElementCacheKey(Element element)
            : this(element.Name, element.Category, element.Culture)
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ElementCacheKey" /> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="category">The category.</param>
        /// <param name="culture">The culture.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
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
        #endregion

        #region Properties
        public string Name { get; private set; }
        public string Category { get; private set; }
        public string Culture { get; private set; }

        public int Hash { get; private set; }
        #endregion

        #region Methods
        public override int GetHashCode()
        {
            return Hash;
        }
        public override bool Equals(object obj)
        {
            return this.Hash == ((ElementCacheKey)obj).GetHashCode();
        }
        #endregion
    }
}
