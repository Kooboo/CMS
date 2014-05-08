#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.DataSource.ValueProvider;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.DataSource
{
    public class DataSourceContext
    {
        public DataSourceContext(Site site, Page page)
        {
            this.Site = site;
            this.Page = page;         
        }
        public Site Site { get; private set; }
        public Page Page { get; private set; }
        public IValueProvider ValueProvider { get; set; }
    }
}
