using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Menu;
using System.Web.Mvc;
using Kooboo.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Contents
{
    public static class MenuExtensions
    {
        public static Menu SetCurrentRepository(this Menu menu, ViewContext viewContext)
        {
            var repository = viewContext.RequestContext.GetRequestValue("repositoryName");
            if (repository != null)
            {
                foreach (var item in menu.Items)
                {
                    SetCurrentRepository(item, repository.ToString());
                }
            }
            return menu;
        }
        private static void SetCurrentRepository(this MenuItem menuItem, string repository)
        {
            menuItem.RouteValues["repositoryName"] = repository;
            if (menuItem.Items != null)
            {
                foreach (var item in menuItem.Items)
                {

                    if (item.Items != null)
                    {
                        SetCurrentRepository(item, repository);
                    }
                }
            }
        }
    }
}