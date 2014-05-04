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
    public class PreMapRequestHandlerEventArgs : SiteEventArgsBase
    {
        public PreMapRequestHandlerEventArgs(HttpContextBase httpContext, Site site)
            : base(httpContext)
        {
            this.Site = site;
        }

        /// <summary>
        /// Gets the site.
        /// </summary>
        /// <value>
        /// The site.
        /// </value>
        public Site Site { get; private set; }
    }
}
