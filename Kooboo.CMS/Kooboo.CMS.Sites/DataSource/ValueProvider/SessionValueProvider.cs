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

namespace Kooboo.CMS.Sites.DataSource.ValueProvider
{
    public class SessionValueProvider : IValueProvider
    {
        public SessionValueProvider(HttpSessionStateBase session)
        {
            this.Session = session;
        }

        public HttpSessionStateBase Session { get; private set; }

        public object GetValue(string name)
        {
            if (Session != null)
            {
                return Session[name];
            }
            return null;
        }
    }
}
