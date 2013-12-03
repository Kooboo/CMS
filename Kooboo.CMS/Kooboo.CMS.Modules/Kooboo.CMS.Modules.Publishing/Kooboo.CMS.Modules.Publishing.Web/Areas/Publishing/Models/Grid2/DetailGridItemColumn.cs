#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2
{
    public class DetailGridItemColumn : GridItemColumn
    {
        public DetailGridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }

        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            if (DataItem is Kooboo.CMS.Common.Persistence.Non_Relational.IIdentifiable)
            {
                var data = (Kooboo.CMS.Common.Persistence.Non_Relational.IIdentifiable)DataItem;
                var linkText = string.Empty;
                var @class = string.Empty;
                if (!string.IsNullOrEmpty(this.GridColumn.PropertyName))
                {
                    linkText = this.PropertyValue == null ? "" : PropertyValue.ToString();
                }
                else
                {
                    @class = "o-icon " + @class;
                }
                return viewContext.HtmlHelper().ActionLink(linkText, "Detail", viewContext.RequestContext.AllRouteValues()
                    .Merge("UUID", data.UUID).Merge("return", viewContext.HttpContext.Request.RawUrl)
                    , new Dictionary<string, object> { { "class", @class } });
            }
            return new MvcHtmlString(string.Empty);
        }
    }
}