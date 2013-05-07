#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Content.Models;
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Common.Persistence.Non_Relational;
namespace Kooboo.CMS.Web.Areas.Contents.Models.Grid2
{
    public class TextFolder_Name_GridItemColumn : GridItemColumn
    {
        public TextFolder_Name_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        { }
        public override IHtmlString RenderItemColumn(ViewContext viewContext)
        {
            var data = ((TextFolder)DataItem).AsActual();
            UrlHelper url = new UrlHelper(viewContext.RequestContext);
            if (!string.IsNullOrEmpty(data.SchemaName))
            {
                return new HtmlString(string.Format(@"<a href=""{0}""><img class='icon {2}' src='{3}'/>{1}</a>"
                , url.Action("Index", "TextContent", viewContext.RequestContext.AllRouteValues().Merge("FolderName", data.FullName).Merge("Folder", data.FullName).Merge("FullName", data.FullName))
                , data.FriendlyText
                , "folder"
                , Kooboo.Web.Url.UrlUtility.ResolveUrl("~/Images/invis.gif")));
            }
            else
            {
                return new HtmlString(string.Format(@"<a href=""{0}""><img class='icon {2}' src='{3}'/>{1}</a>"
                , url.Action("Index", "TextFolder", viewContext.RequestContext.AllRouteValues().Merge("FolderName", data.FullName).Merge("Folder", data.FullName).Merge("FullName", data.FullName))
                , data.FriendlyText
                , "folder"
                , Kooboo.Web.Url.UrlUtility.ResolveUrl("~/Images/invis.gif")));
            }
        }
    }
}