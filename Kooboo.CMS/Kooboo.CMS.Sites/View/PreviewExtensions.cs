#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace Kooboo.CMS.Sites.View
{
    public static class PreviewExtensions
    {
        public static IHtmlString Preview(this FrontUrlHelper frontUrl, Page page)
        {
            return Preview(frontUrl, page, null);
        }
        public static IHtmlString Preview(this FrontUrlHelper frontUrl, Page page, object values)
        {
            if (page != null)
            {
                page = page.AsActual();
            }
            var urlHelper = frontUrl.Url;
            var pageUrl = urlHelper.Content("~/");
            if (page != null && !page.IsDefault)
            {
                pageUrl = urlHelper.Content("~/" + page.VirtualPath);
            }
            var previewUrl = FrontUrlHelper.WrapperUrl(pageUrl, frontUrl.Site, frontUrl.RequestChannel).ToString();
            if (values != null)
            {
                RouteValueDictionary routeValues = new RouteValueDictionary(values);

                foreach (var item in routeValues)
                {
                    if (item.Value != null)
                    {
                        previewUrl = Kooboo.Common.Web.UrlUtility.AddQueryParam(previewUrl, item.Key, item.Value.ToString());
                    }
                }
            }
            return new HtmlString(previewUrl);
        }

    }
}
