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
using Kooboo.Common.Globalization;
using Kooboo.Common.Web.Grid;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;
namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class Version_GridItemColumn : GridItemColumn
    {
        public Version_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            if (Site.Current.EnableVersioning.HasValue ? Site.Current.EnableVersioning.Value : true)
            {
                var inheritableData = DataItem as Kooboo.CMS.Sites.Models.IInheritable;
                if (inheritableData == null || inheritableData.IsLocalized(Site.Current))
                {
                    var data = (IIdentifiable)DataItem;

                    var linkText = "Version".Localize();

                    return viewContext.HtmlHelper().ActionLink(linkText, "Version", viewContext.RequestContext.AllRouteValues().Merge("UUID", data.UUID),
                        new Dictionary<string, object> { { "class", "o-icon version dialog-link" } });
                }
            }

            return new HtmlString("");
        }
    }
}