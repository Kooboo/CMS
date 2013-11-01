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
    public class OutgoingLogLinkGridItemColumn : GridItemColumn
    {
        public OutgoingLogLinkGridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }

        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            var data = (OutgoingQueue)DataItem;
            var linkText = "Logs".Localize();

            return viewContext.HtmlHelper().ActionLink(linkText, "Logs",viewContext.RequestContext.AllRouteValues()
                .Merge("UUID", data.UUID).Merge("return", viewContext.HttpContext.Request.RawUrl));
        }
    }
}