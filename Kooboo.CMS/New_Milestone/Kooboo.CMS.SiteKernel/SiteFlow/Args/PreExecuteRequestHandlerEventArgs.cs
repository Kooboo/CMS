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
    public class PreExecuteRequestHandlerEventArgs : EventArgsBase
    {
        public PreExecuteRequestHandlerEventArgs(HttpContextBase httpContext, Site site, IRequestHandler requestHandler)
            : base(httpContext)
        {
            this.Site = site;
            this.RequestHandler = requestHandler;
        }
        /// <summary>
        /// Gets the site.
        /// </summary>
        /// <value>
        /// The site.
        /// </value>
        public Site Site { get; private set; }

        /// <summary>
        /// Gets or sets the request handler.
        /// </summary>
        /// <value>
        /// The request handler.
        /// </value>
        public IRequestHandler RequestHandler { get; private set; }
    }
}
