using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.Specialized;
using System.Collections;
using System.Runtime.Serialization;

namespace Kooboo.Collections
{
    public class ReadonlyNameValueCollection : NameValueCollection
    {
        #region ctor
		public ReadonlyNameValueCollection()
        {

        }
        public ReadonlyNameValueCollection(IEqualityComparer equalityComparer)
            : base(equalityComparer)
        {
        }
        public ReadonlyNameValueCollection(NameValueCollection col)
            : base()
        {
            this.Add(col);
        }
        public ReadonlyNameValueCollection(int capacity)
            : base(capacity)
        {
        }
        public ReadonlyNameValueCollection(int capacity, IEqualityComparer equalityComparer)
            : base(capacity, equalityComparer)
        {
        }
        public ReadonlyNameValueCollection(int capacity, NameValueCollection col)
            : base()
        {
            if (col == null)
            {
                throw new ArgumentNullException("col");
            }
            this.Add(col);
        }
        protected ReadonlyNameValueCollection(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        } 
	#endregion

        public void MakeReadOnly()
        {
            base.IsReadOnly = true;
        }
        public void MakeReadWrite()
        {
            base.IsReadOnly = false;
        }
    }
}
