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
using System.Threading.Tasks;
using System.Web;

namespace Kooboo.CMS.SiteKernel.SiteFlow.Args
{
    public class SiteEventArgsBase : EventArgs
    {
        public SiteEventArgsBase(HttpContextBase httpContext)
        {
            this.HttpContext = httpContext;
        }
        /// <summary>
        /// Gets or sets the HTTP context.
        /// </summary>
        /// <value>
        /// The HTTP context.
        /// </value>
        public HttpContextBase HttpContext { get; private set; }
    }
}
