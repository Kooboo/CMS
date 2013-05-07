#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion

using Kooboo.CMS.Sites.Services;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Models.DataSources
{
    public class FileExtensionsDataSource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            List<SelectListItem> items = new List<SelectListItem>();

            var type = requestContext.GetRequestValue("type").ToLower();

            var fileManager = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<FileManager>(type);

            if (fileManager is ThemeManager)
            {
                items.Add(new SelectListItem() { Text = ".css", Value = ".css" });
                items.Add(new SelectListItem() { Text = ".rule", Value = ".rule" });

            }
            if (fileManager is ScriptManager)
            {
                items.Add(new SelectListItem() { Text = ".js", Value = ".js" });
            }
            if (fileManager is CustomFileManager)
            {
                items.Add(new SelectListItem() { Text = ".js", Value = ".js" });
                items.Add(new SelectListItem() { Text = ".css", Value = ".css" });
                items.Add(new SelectListItem() { Text = ".txt", Value = ".txt" });
            }
            return items;
        }
    }
}