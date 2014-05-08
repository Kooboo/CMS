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
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataSource.ValueProvider
{
    public class NameValueCollectionValueProvider : IValueProvider
    {
        public NameValueCollectionValueProvider(NameValueCollection queryString)
        {
            this.QueryString = queryString;
        }
        public NameValueCollection QueryString { get; private set; }

        public object GetValue(string name)
        {
            return QueryString[name];
        }
    }
}
