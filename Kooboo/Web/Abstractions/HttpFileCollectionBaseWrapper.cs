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
using System.Runtime.Serialization;
using System.Collections.Specialized;

namespace Kooboo.Web
{
    public class HttpFileCollectionBaseWrapper : HttpFileCollectionBase
    {
        // Fields
        private HttpFileCollectionBase _collection;

        // Methods
        public HttpFileCollectionBaseWrapper(HttpFileCollectionBase httpFileCollection)
        {
            if (httpFileCollection == null)
            {
                throw new ArgumentNullException("httpFileCollection");
            }
            this._collection = httpFileCollection;
        }

        public override void CopyTo(Array dest, int index)
        {
            this._collection.CopyTo(dest, index);
        }

        public override HttpPostedFileBase Get(int index)
        {
            return _collection.Get(index);
        }

        public override HttpPostedFileBase Get(string name)
        {
            return _collection.Get(name);
        }

        public override IEnumerator GetEnumerator()
        {
            return this._collection.GetEnumerator();
        }

        public override string GetKey(int index)
        {
            return this._collection.GetKey(index);
        }

        // Properties
        public override string[] AllKeys
        {
            get
            {
                return this._collection.AllKeys;
            }
        }

        public override int Count
        {
            get
            {
                return this._collection.Count;
            }
        }

        public override bool IsSynchronized
        {
            get
            {
                return this._collection.IsSynchronized;
            }
        }

        public override HttpPostedFileBase this[string name]
        {
            get
            {
                return _collection[name];
            }
        }

        public override HttpPostedFileBase this[int index]
        {
            get
            {
                return _collection[index];
            }
        }

        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return this._collection.Keys;
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
