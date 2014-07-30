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

namespace Kooboo.CMS.SiteKernel.FrontAPI
{
    public static class PageUrlExtensions
    {
        public static IHtmlString PageUrl(this IFrontUrlHelper frontUrlHelper, string urlMapKey)
        {
            return PageUrl(frontUrlHelper, urlMapKey, null);
        }

        public static IHtmlString PageUrl(this IFrontUrlHelper frontUrlHelper, string urlMapKey, object values)
        {
            Page page;
            return PageUrl(frontUrlHelper, urlMapKey, values, out page);
        }
        public static IHtmlString PageUrl(this IFrontUrlHelper frontUrlHelper, string urlMapKey, object values, out Page page)
        {
            var url = frontUrlHelper.GeneratePageUrl(urlMapKey, values, (site, key) => null, out page);
            return url;
        }
    }
}
