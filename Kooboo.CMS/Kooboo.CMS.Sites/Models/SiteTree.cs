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

namespace Kooboo.CMS.Sites.Models
{
    public class SiteNode
    {
        public Site Site { get; set; }
        public bool IsOnLine
        {
            get
            {
                return Services.ServiceFactory.SiteManager.IsOnline(Site.FullName);
            }
        }
        public IEnumerable<SiteNode> Children { get; set; }
    }
    public class SiteTree
    {
        public SiteNode Root { get; set; }
    }
}
