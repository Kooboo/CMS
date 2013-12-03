﻿
using Kooboo.CMS.Common.Persistence.Relational;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Models;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.ViewModels.Grid2
{
    public class EditGridActionItemColumn : GridItemColumn
    {
        public EditGridActionItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            if (DataItem is IEntity)
            {
                var data = (IEntity)DataItem;

                var linkText = "Edit".Localize();
                var @class = Class;
                if (!string.IsNullOrEmpty(this.GridColumn.PropertyName))
                {
                    linkText = this.PropertyValue == null ? "" : PropertyValue.ToString();
                }
                var url = viewContext.UrlHelper().Action("Edit"
                    , viewContext.RequestContext.AllRouteValues().Merge("Id", data.Id).Merge("return", viewContext.HttpContext.Request.RawUrl));

                return new HtmlString(string.Format("<a href='{0}'><img class='icon {2}' src='{3}'/> {1}</a>", url, linkText,
                    @class, Kooboo.Web.Url.UrlUtility.ResolveUrl("~/Images/invis.gif")));
            }

            return new HtmlString("");
        }
        protected virtual string Class
        {
            get { return ""; }
        }
    }
}