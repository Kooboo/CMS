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
using System.Collections.Specialized;
using System.Runtime.Serialization;

namespace Kooboo.Collections
{
    /// <summary>
    /// 
    /// </summary>
    public class ReadonlyNameValueCollection : NameValueCollection
    {
        #region .ctor
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadonlyNameValueCollection" /> class.
        /// </summary>
		public ReadonlyNameValueCollection()
        {

        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadonlyNameValueCollection" /> class.
        /// </summary>
        /// <param name="equalityComparer">The equality comparer.</param>
        public ReadonlyNameValueCollection(IEqualityComparer equalityComparer)
            : base(equalityComparer)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadonlyNameValueCollection" /> class.
        /// </summary>
        /// <param name="col">The col.</param>
        public ReadonlyNameValueCollection(NameValueCollection col)
            : base()
        {
            this.Add(col);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadonlyNameValueCollection" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public ReadonlyNameValueCollection(int capacity)
            : base(capacity)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadonlyNameValueCollection" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="equalityComparer">The equality comparer.</param>
        public ReadonlyNameValueCollection(int capacity, IEqualityComparer equalityComparer)
            : base(capacity, equalityComparer)
        {
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadonlyNameValueCollection" /> class.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        /// <param name="col">The col.</param>
        /// <exception cref="System.ArgumentNullException"></exception>
        public ReadonlyNameValueCollection(int capacity, NameValueCollection col)
            : base()
        {
            if (col == null)
            {
                throw new ArgumentNullException("col");
            }
            this.Add(col);
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadonlyNameValueCollection" /> class.
        /// </summary>
        /// <param name="info">The info.</param>
        /// <param name="context">The context.</param>
        protected ReadonlyNameValueCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        } 
	#endregion

        #region Methods
        /// <summary>
        /// Makes the read only.
        /// </summary>
        public void MakeReadOnly()
        {
            base.IsReadOnly = true;
        }
        /// <summary>
        /// Makes the read write.
        /// </summary>
        public void MakeReadWrite()
        {
            base.IsReadOnly = false;
        } 
        #endregion
    }
}
