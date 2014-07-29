﻿#region License
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
using Kooboo.Common.Globalization;
using Kooboo.Common.ObjectContainer.Dependency;
namespace Kooboo.CMS.Web2
{
    [Dependency(typeof(Kooboo.Common.Web.IHttpApplicationEvents), Key = "ReferrerCheckingModule")]
    public class ReferrerCheckingModule : Kooboo.Common.Web.HttpApplicationEvents
    {
        public override void Application_BeginRequest(object sender, EventArgs e)
        {
            base.Application_BeginRequest(sender, e);

            var httpContext = HttpContext.Current;
            if (httpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                bool allowCrossDomainPost = false;
                var crossdomainPostSetting = System.Configuration.ConfigurationManager.AppSettings["AllowCrossDomainPost"];
                if (!string.IsNullOrEmpty(crossdomainPostSetting))
                {
                    allowCrossDomainPost = crossdomainPostSetting.ToLower() == "true";
                }

                if (!allowCrossDomainPost && httpContext.Request.UrlReferrer != null && httpContext.Request.UrlReferrer.Host != httpContext.Request.Url.Host)
                {
                    throw new HttpException("The cross-domain POST request is not allowed.".Localize());
                }
            }
        }
    }
}
