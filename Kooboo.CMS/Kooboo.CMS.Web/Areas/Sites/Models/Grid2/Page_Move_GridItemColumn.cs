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
using Kooboo.CMS.Web.Grid2;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc.Html;

namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class Page_Move_GridItemColumn : GridItemColumn
    {
        public Page_Move_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var inheritableData = DataItem as Kooboo.CMS.Sites.Models.IInheritable;
            if (inheritableData == null || inheritableData.IsLocalized(Site.Current))
            {
                var data = (IIdentifiable)DataItem;

                var linkText = "Move".Localize();

                //routeValueDictionary["ReturnUrl"] = (new UrlHelper(viewContext.RequestContext)).Action("Index", viewContext.RequestContext.AllRouteValues());
                //routeValueDictionary["from"] = "page";

                return viewContext.HtmlHelper().ActionLink(linkText, "Move", viewContext.RequestContext.AllRouteValues().Merge("UUID", data.UUID),
                    new Dictionary<string, object> { { "class", "o-icon move-page dialog-link" } });
            }
           return  new HtmlString("");
        }
    }
}