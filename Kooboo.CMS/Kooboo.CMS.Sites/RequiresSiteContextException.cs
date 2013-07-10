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

namespace Kooboo.CMS.Sites
{
    public class RequiresSiteContextException : InvalidOperationException
    {
        public RequiresSiteContextException()
            : base("Requires the site context, Site.Current can not be null.")
        {
        }
    }
}
