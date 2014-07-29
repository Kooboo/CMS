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
    public class PostMapSiteEventArgs : SiteEventArgsBase
    {
        /// <summary>
        /// Pres the map site event arguments.
        /// </summary>
        /// <param name="httpContext">The HTTP context.</param>
        public PostMapSiteEventArgs(HttpContextBase httpContext, Site site)
            :base(httpContext)
        {
            this.Site = site;
        }       

        /// <summary>
        /// Site对象运行被替换。
        /// 用于：A/B Site test
        /// </summary>
        /// <value>
        /// The site.
        /// </value>
        public Site Site { get; set; }
    }
}
