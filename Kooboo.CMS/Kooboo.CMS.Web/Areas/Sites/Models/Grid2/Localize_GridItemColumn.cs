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
    public class Localize_GridItemColumn : GridItemColumn
    {
        public Localize_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var inheritable = DataItem as IInheritable;
            var data = (IIdentifiable)DataItem;
            if (inheritable != null)
            {
                var localized = inheritable.IsLocalized(Site.Current);
                if (localized)
                {
                    var hasParent = Site.Current.Parent != null;
                    if (hasParent)
                    {
                        return viewContext.HtmlHelper().ActionLink("Unlocalize".Localize(), "Unlocalize", viewContext.RequestContext.AllRouteValues().Merge("UUID", data.UUID),
                       new Dictionary<string, object> { { "class", "o-icon unlocalize actionCommand" }, { "ConfirmMessage", "Are you sure you want to unlocalize this item?".Localize() } });
                    }
                }
                else
                {
                    var linkText = "Localize".Localize();

                    return viewContext.HtmlHelper().ActionLink(linkText, "Localize", viewContext.RequestContext.AllRouteValues().Merge("UUID", data.UUID),
                        new Dictionary<string, object> { { "class", "o-icon localize" }, { "ConfirmMessage", "Are you sure you want to localize this item?".Localize() } });
                }
            }
            return new HtmlString("");
        }
    }
}