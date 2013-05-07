#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using System.IO;

namespace Kooboo.Web
{
    class HttpStaticObjectsCollectionBaseWrapper : HttpStaticObjectsCollectionBase
    {
        // Fields
        private HttpStaticObjectsCollectionBase _collection;

        // Methods
        public HttpStaticObjectsCollectionBaseWrapper(HttpStaticObjectsCollectionBase httpStaticObjectsCollection)
        {
            if (httpStaticObjectsCollection == null)
            {
                throw new ArgumentNullException("httpStaticObjectsCollection");
            }
            this._collection = httpStaticObjectsCollection;
        }

        public override void CopyTo(Array array, int index)
        {
            this._collection.CopyTo(array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            return this._collection.GetEnumerator();
        }

        public override object GetObject(string name)
        {
            return this._collection.GetObject(name);
        }

        public override void Serialize(BinaryWriter writer)
        {
            this._collection.Serialize(writer);
        }

        // Properties
        public override int Count
        {
            get
            {
                return this._collection.Count;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return this._collection.IsReadOnly;
            }
        }

        public override bool IsSynchronized
        {
            get
            {
                return this._collection.IsSynchronized;
            }
        }

        public override object this[string name]
        {
            get
            {
                return this._collection[name];
            }
        }

        public override bool NeverAccessed
        {
            get
            {
                return this._collection.NeverAccessed;
            }
        }

        public override object SyncRoot
        {
            get
            {
                return this._collection.SyncRoot;
            }
        }
    }

}
