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

namespace Kooboo.Web
{
    public class IHtmlStringComparer : IEqualityComparer<IHtmlString>
    {
        public IHtmlStringComparer()
            : this(true)
        { }
        public IHtmlStringComparer(bool ignoreCase)
        {
            this.IgnoreCase = ignoreCase;
        }
        public bool IgnoreCase { get; private set; }
        #region IEqualityComparer<IHtmlString> Members

        public bool Equals(IHtmlString x, IHtmlString y)
        {
            if (x == y)
            {
                return true;
            }
            if (string.Compare(x.ToString(), y.ToString(), IgnoreCase) == 0)
            {
                return true;
            }
            return false;
        }

        public int GetHashCode(IHtmlString obj)
        {
            return obj.ToString().GetHashCode();
        }

        #endregion
    }
}
