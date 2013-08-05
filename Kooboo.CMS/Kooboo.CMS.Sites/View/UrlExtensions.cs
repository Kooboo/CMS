#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.View
{
    public static class UrlExtensions
    {
        public static FrontUrlHelper FrontUrl(this UrlHelper url)
        {
            if (Page_Context.Current.Initialized)
            {
                return Page_Context.Current.FrontUrl;
            }
            //Throw "Unable to cast object of type 'Kooboo.CMS.Sites.Extension.ModuleArea.ModuleHttpRequest' to type 'Kooboo.CMS.Sites.Web.FrontHttpRequestWrapper'."
            FrontRequestChannel requestChannel = FrontRequestChannel.Unknown;
            if (url.RequestContext.HttpContext.Request is FrontHttpRequestWrapper)
            {
                requestChannel = ((FrontHttpRequestWrapper)url.RequestContext.HttpContext.Request).RequestChannel;
            }
            return new FrontUrlHelper(url, Site.Current, requestChannel);
        }
        public static FrontUrlHelper FrontUrl(this UrlHelper url, Site site, FrontRequestChannel requestChannel = FrontRequestChannel.Host)
        {
            return new FrontUrlHelper(url, site, requestChannel);
        }
    }
}
