#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.Globalization;
using Kooboo.Common.Web.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class Script_FileName_GridItemColumn : GridItemColumn
    {
        public Script_FileName_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var script = (ScriptFile)DataItem;
            if (script.IsLocalized(Site.Current))
            {
                UrlHelper url = new UrlHelper(viewContext.RequestContext);

                return new HtmlString(string.Format(@"<a href=""{0}"" class=""f-icon file js dialog-link"" title=""{1}"">{2}</a>", url.Action("Edit", viewContext.RequestContext.AllRouteValues().Merge("FileName", ((ScriptFile)DataItem).FileName)), "Edit".Localize(), PropertyValue));
            }
            else
            {
                return new HtmlString(string.Format("{0}", PropertyValue));
            }
        }
    }
}