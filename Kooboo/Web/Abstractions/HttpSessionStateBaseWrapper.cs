using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Collections;
using System.Collections.Specialized;
using System.Web.SessionState;

namespace Kooboo.Web
{
    public class HttpSessionStateBaseWrapper : HttpSessionStateBase
    {
        // Fields
        private readonly HttpSessionStateBase _session;

        // Methods
        public HttpSessionStateBaseWrapper(HttpSessionStateBase httpSessionState)
        {
            if (httpSessionState == null)
            {
                throw new ArgumentNullException("httpSessionState");
            }
            this._session = httpSessionState;
        }

        public override void Abandon()
        {
            this._session.Abandon();
        }

        public override void Add(string name, object value)
        {
            this._session.Add(name, value);
        }

        public override void Clear()
        {
            this._session.Clear();
        }

        public override void CopyTo(Array array, int index)
        {
            this._session.CopyTo(array, index);
        }

        public override IEnumerator GetEnumerator()
        {
            return this._session.GetEnumerator();
        }

        public override void Remove(string name)
        {
            this._session.Remove(name);
        }

        public override void RemoveAll()
        {
            this._session.RemoveAll();
        }

        public override void RemoveAt(int index)
        {
            this._session.RemoveAt(index);
        }

        // Properties
        public override int CodePage
        {
            get
            {
                return this._session.CodePage;
            }
            set
            {
                this._session.CodePage = value;
            }
        }

        public override HttpSessionStateBase Contents
        {
            get
            {
                return this;
            }
        }

        public override HttpCookieMode CookieMode
        {
            get
            {
                return this._session.CookieMode;
            }
        }

        public override int Count
        {
            get
            {
                return this._session.Count;
            }
        }

        public override bool IsCookieless
        {
            get
            {
                return this._session.IsCookieless;
            }
        }

        public override bool IsNewSession
        {
            get
            {
                return this._session.IsNewSession;
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return this._session.IsReadOnly;
            }
        }

        public override bool IsSynchronized
        {
            get
            {
                return this._session.IsSynchronized;
            }
        }

        public override object this[string name]
        {
            get
            {
                return this._session[name];
            }
            set
            {
                this._session[name] = value;
            }
        }

        public override object this[int index]
        {
            get
            {
                return this._session[index];
            }
            set
            {
                this._session[index] = value;
            }
        }

        public override NameObjectCollectionBase.KeysCollection Keys
        {
            get
            {
                return this._session.Keys;
            }
        }

        public override int LCID
        {
            get
            {
                return this._session.LCID;
            }
            set
            {
                this._session.LCID = value;
            }
        }

        public override SessionStateMode Mode
        {
            get
            {
                return this._session.Mode;
            }
        }

        public override string SessionID
        {
            get
            {
                return this._session.SessionID;
            }
        }

        public override HttpStaticObjectsCollectionBase StaticObjects
        {
            get
            {
                return new HttpStaticObjectsCollectionBaseWrapper(this._session.StaticObjects);
            }
        }

        public override object SyncRoot
        {
            get
            {
                return this._session.SyncRoot;
            }
        }

        public override int Timeout
        {
            get
            {
                return this._session.Timeout;
            }
            set
            {
                this._session.Timeout = value;
            }
        }
    }

}
