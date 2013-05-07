using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.Common.Formula
{
    public class NameValueCollectionProvider : IValueProvider
    {
        NameValueCollection _values;
        public NameValueCollectionProvider(NameValueCollection values)
        {
            this._values = values;
        }
        public object GetValue(string name)
        {
            return this._values[name];
        }
    }
}
