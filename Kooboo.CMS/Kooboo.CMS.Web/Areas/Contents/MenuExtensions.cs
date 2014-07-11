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
using System.Web;
using Kooboo.Common.Web.Menu;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Contents
{
    public static class MenuExtensions
    {
        public static Kooboo.Common.Web.Menu.Menu SetCurrentRepository(this Kooboo.Common.Web.Menu.Menu menu, ViewContext viewContext)
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