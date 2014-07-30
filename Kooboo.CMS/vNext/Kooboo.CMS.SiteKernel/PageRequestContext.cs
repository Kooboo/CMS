#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.SiteKernel.Models;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kooboo.CMS.SiteKernel
{
    public class PageRequestContext
    {
        public FrontRequestChannel RequestChannel { get; private set; }
        public Page Page { get; private set; }

        public Site Site { get; private set; }

        public NameValueCollection AllQueryString { get; private set; }
    }
}
