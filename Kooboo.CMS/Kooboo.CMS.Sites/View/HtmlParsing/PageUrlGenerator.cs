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

namespace Kooboo.CMS.Sites.View.HtmlParsing
{
    [Kooboo.Common.ObjectContainer.Dependency.Dependency(typeof(IPageUrlGenerator))]
    public class PageUrlGenerator : IPageUrlGenerator
    {
        public string PageUrl(string pageName, System.Web.Routing.RouteValueDictionary routeValues)
        {
            return Page_Context.Current.Url.FrontUrl().PageUrl(pageName, routeValues).ToString();
        }
    }
}
