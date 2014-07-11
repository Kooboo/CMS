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
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Common.Globalization;

using Kooboo.Common.Web.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class Page_Preview_GridItemColumn : GridItemColumn
    {
        public Page_Preview_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var isStaticPage = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.IsStaticPage(Kooboo.CMS.Sites.Models.Site.Current, (Page)DataItem);

            UrlHelper urlHelper = new UrlHelper(viewContext.RequestContext);
            var href = FrontUrlHelper.Preview(urlHelper, Kooboo.CMS.Sites.Models.Site.Current, (Page)DataItem, null);

            if (!isStaticPage)
            {
                return new HtmlString(PropertyValue == null ? "" : PropertyValue.ToString());
            }

            return new HtmlString(string.Format(@"<a href=""{0}"" target=""_blank"" class=""o-icon preview"" title=""{2}"">{1}</a>", href, PropertyValue, PropertyValue));
        }
    }
}