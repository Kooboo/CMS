using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Menu;
using System.Web.Mvc;
using Kooboo.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites
{
    public static class MenuExtensions
    {
        public static Menu SetCurrentSite(this Menu menu, ViewContext viewContext)
        {
            var siteName = viewContext.RequestContext.GetRequestValue("siteName");
            if (siteName != null)
            {
                foreach (var item in menu.Items)
                {
                    SetCurrentRepository(item, siteName.ToString());
                }   
            }
            return menu;
        }
        private static void SetCurrentRepository(this MenuItem menuItem, string siteName)
        {
            menuItem.RouteValues["siteName"] = siteName;
            if (menuItem.Items != null)
            {
                foreach (var item in menuItem.Items)
                {

                    if (item.Items != null)
                    {
                        SetCurrentRepository(item, siteName);
                    }
                }
            }
        }      
    }
}