#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.Common.Globalization;
using Kooboo.Common.Web.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace Kooboo.CMS.Web.Grid2
{
    public class DialogEditGridActionItemColumn : GridItemColumn
    {
        public DialogEditGridActionItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            if (DataItem is IIdentifiable)
            {
                var data = (IIdentifiable)DataItem;
                var linkText = "Edit".Localize();
                var @class = Class;
                if (!string.IsNullOrEmpty(this.GridColumn.PropertyName))
                {
                    linkText = this.PropertyValue == null ? "" : PropertyValue.ToString();
                }
                else
                {
                    @class = "o-icon edit " + @class;
                }
                return viewContext.HtmlHelper().ActionLink(linkText, "Edit", viewContext.RequestContext.AllRouteValues().Merge("UUID", data.UUID), new Dictionary<string, object> { { "class", @class } });
            }

            return new HtmlString("");

        }
        protected virtual string Class
        {
            get
            {
                return "dialog-link";
            }
        }
    }
}