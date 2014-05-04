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
using System.Web.Mvc;

namespace Kooboo.CMS.SiteKernel.SiteFlow.Args
{
    public class PostMapPageEventArgs : EventArgs
    {
        public PostMapPageEventArgs(ControllerContext controllerContext, Site site, Page page)
        {
            this.ControllerContext = controllerContext;
            this.Site = site;
            this.Page = page;
        }
        public ControllerContext ControllerContext { get; private set; }

        public Site Site { get; private set; }

        /// <summary>
        /// Gets or sets the page.
        /// Page对象可以被替换，用在A/B page test的场景里面。
        /// </summary>
        /// <value>
        /// The page.
        /// </value>
        public Page Page { get; set; }
    }
}
