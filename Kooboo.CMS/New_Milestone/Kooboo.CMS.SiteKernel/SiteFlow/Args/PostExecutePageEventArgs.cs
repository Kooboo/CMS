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
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.SiteKernel.SiteFlow.Args
{
    public class PostExecutePageEventArgs : EventArgsBase
    {
        public PostExecutePageEventArgs(HttpContextBase httpContext, Site site, Page page)
            : base(httpContext)
        {
            this.Site = site;
            this.Page = page;
        }
        public Site Site { get; private set; }
        /// <summary>
        /// Gets or sets the page.
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public Page Page { get; private set; }
    }
}
