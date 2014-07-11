#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Common.Web.SelectList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class ThemesDataSource : ISelectListDataSource
    {
        public ThemeManager ThemeFileManager { get; private set; }
        public ThemesDataSource(ThemeManager themeFileManager)
        {
            ThemeFileManager = themeFileManager;
        }
        #region IDropDownListDataSource Members

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var siteName = requestContext.GetRequestValue("siteName");
            if (string.IsNullOrEmpty(siteName))
            {
                siteName = requestContext.GetRequestValue("parent");
            }
            if (!string.IsNullOrEmpty(siteName))
            {
                var site = new Site(siteName);
                foreach (var item in ThemeFileManager.AllThemes(site))
                {
                    yield return new SelectListItem() { Text = item, Value = item };
                }
            }
        }

        #endregion
    }
}