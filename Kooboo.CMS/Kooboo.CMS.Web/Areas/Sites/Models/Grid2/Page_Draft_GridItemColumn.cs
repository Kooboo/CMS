#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Common.Globalization;

using Kooboo.Common.Web.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;
using System.Web.Mvc;
using System.Web.Routing;
namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class Page_Draft_GridItemColumn : GridItemColumn
    {
        public Page_Draft_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var inheritableData = DataItem as Kooboo.CMS.Sites.Models.IInheritable;
            if (inheritableData == null || inheritableData.IsLocalized(Site.Current))
            {
                var page = (Page)DataItem;
                if (ServiceFactory.PageManager.HasDraft(page))
                {
                    var data = (IIdentifiable)DataItem;

                    var linkText = "Draft".Localize();

                    return viewContext.HtmlHelper().ActionLink(linkText, "Draft", viewContext.RequestContext.AllRouteValues().Merge("UUID", data.UUID).Merge("return", viewContext.HttpContext.Request.RawUrl),
                        new Dictionary<string, object>());
                }
            }
            return new HtmlString("-");
        }
    }
}