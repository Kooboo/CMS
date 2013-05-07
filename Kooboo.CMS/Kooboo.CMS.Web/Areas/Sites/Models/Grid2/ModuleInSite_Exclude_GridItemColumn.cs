#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc;
using System.Web.Mvc.Html;
using Kooboo.Globalization;
namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class ModuleInSite_Exclude_GridItemColumn : GridItemColumn
    {
        public ModuleInSite_Exclude_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var module = (ModuleListInSiteModel)DataItem;
            if (module.Included)
            {
                return viewContext.HtmlHelper().ActionLink("Exclude".Localize(), "Exclude", viewContext.RequestContext.AllRouteValues().Merge("ModuleName", module.ModuleName),
                    new Dictionary<string, object> { { "class", "o-icon delete ajax-post-link" }, { "confirm", "Are you sure you want to exclude this module from the site?".Localize() } });
            }
            return new HtmlString("");
        }
    }
}