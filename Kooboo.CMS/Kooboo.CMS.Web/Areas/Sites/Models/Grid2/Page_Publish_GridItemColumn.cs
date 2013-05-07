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
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class Page_Publish_GridItemColumn : BooleanGridItemColumn
    {
        public Page_Publish_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var page = (Page)DataItem;
            var allowToPublish = ServiceFactory.UserManager.Authorize(Site.Current, viewContext.HttpContext.User.Identity.Name, Kooboo.CMS.Account.Models.Permission.Sites_Page_PublishPermission);
            allowToPublish = allowToPublish && page.IsLocalized(Site.Current);
            if (allowToPublish)
            {
                string url = "";
                var published = (bool)PropertyValue;

                var urlHelper = new UrlHelper(viewContext.RequestContext);
                string tip = "Click to {0}";
                string @class = "";
                if (published)
                {
                    url = urlHelper.Action("Unpublish", viewContext.RequestContext.AllRouteValues().Merge("FullName", page.FullName));
                    tip = string.Format(tip, "unpublish").Localize();
                    @class = "actionCommand";
                }
                else
                {
                    url = urlHelper.Action("Publish", viewContext.RequestContext.AllRouteValues().Merge("FullName", page.FullName));
                    tip = string.Format(tip, "publish").Localize();
                    @class = "dialog-link";
                }
                return new HtmlString(string.Format(@"<a class=""o-icon {0} {1} "" href=""{2}"" title=""{3}"">{1}</a>"
                    , GetIconClass(PropertyValue)
                    , @class
                    , url
                    , tip));

            }
            else
            {
                return base.RenderItemColumn(viewContext);
            }
        }
    }
}