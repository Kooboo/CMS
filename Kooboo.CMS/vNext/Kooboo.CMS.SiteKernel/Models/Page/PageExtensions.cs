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

using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Common.Web;
namespace Kooboo.CMS.SiteKernel.Models
{
    public static class PageExtensions
    {
        public static string GetVirutalPath(this Page page)
        {
            var segment = page.Name;
            string virtualPath = "";
            var route = page.Route;
            var parent = page.Parent;
            var site = page.Site;
            if (page.Route != null && !string.IsNullOrEmpty(route.Identifier))
            {
                if ((route.Identifier.StartsWith("#") || route.Identifier == "*") && parent != null)
                {
                    return parent.LastVersion(site).AsActual().GetVirutalPath();
                }
                else if (route.Identifier.StartsWith("/"))
                {
                    return route.Identifier;
                }
                else
                {
                    if (route != null && !string.IsNullOrEmpty(route.Identifier) && !route.Identifier.StartsWith("/"))
                    {
                        segment = route.Identifier;
                    }
                }
            }
            if (parent != null)
            {
                virtualPath = UrlUtility.Combine(parent.LastVersion(site).AsActual().GetVirutalPath(), segment);
            }
            else
            {
                virtualPath = segment;
            }
            if (!virtualPath.StartsWith("/"))
            {
                virtualPath = "/" + virtualPath;
            }
            return virtualPath;
        }

        public static string GetLinkText(this Page page)
        {
            if (page.Navigation != null && !string.IsNullOrEmpty(page.Navigation.DisplayText))
            {
                return page.Navigation.DisplayText;
            }
            return page.Name;
        }
    }
}
