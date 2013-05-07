#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
namespace Kooboo.CMS.Web.Areas.Account.Models.Grid2
{
    public class User_ResetPassword_GridItemColumn : GridItemColumn
    {
        public User_ResetPassword_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            var data = (User)DataItem;
            var linkText = "Reset password".Localize();
            var @class = "o-icon password dialog-link";
            return viewContext.HtmlHelper().ActionLink(linkText, "ResetPassword", viewContext.RequestContext.AllRouteValues().Merge("userName", data.UserName), new Dictionary<string, object> { { "class", @class } });
        }
    }
}