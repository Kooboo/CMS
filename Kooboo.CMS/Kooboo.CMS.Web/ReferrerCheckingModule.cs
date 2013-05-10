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
using System.Web;
using Kooboo.Globalization;
using Kooboo.CMS.Common.Runtime.Dependency;
namespace Kooboo.CMS.Web
{
    [Dependency(typeof(Kooboo.CMS.Common.IHttpApplicationEvents), Key = "ReferrerCheckingModule")]
    public class ReferrerCheckingModule : Kooboo.CMS.Common.HttpApplicationEvents
    {
        public override void Application_BeginRequest(object sender, EventArgs e)
        {
            base.Application_BeginRequest(sender, e);

            var httpContext = HttpContext.Current;
            if (httpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                if (httpContext.Request.UrlReferrer.Host != httpContext.Request.Url.Host)
                {
                    throw new HttpException("The cross-domain POST request is not allowed.".Localize());
                }
            }
        }
    }
}
