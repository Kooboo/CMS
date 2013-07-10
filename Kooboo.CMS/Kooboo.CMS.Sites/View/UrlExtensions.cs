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
            var frontHttpRequest = (FrontHttpRequestWrapper)url.RequestContext.HttpContext.Request;
            return new FrontUrlHelper(url, frontHttpRequest.Site, frontHttpRequest.RequestChannel);
        }
        public static FrontUrlHelper FrontUrl(this UrlHelper url, Site site, FrontRequestChannel requestChannel = FrontRequestChannel.Host)
        {
            return new FrontUrlHelper(url, site, requestChannel);
        }
    }
}
