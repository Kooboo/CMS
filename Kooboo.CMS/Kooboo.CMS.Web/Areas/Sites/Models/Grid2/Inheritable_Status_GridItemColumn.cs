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
    public class Inheritable_Status_GridItemColumn : GridItemColumn
    {
        public Inheritable_Status_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {

        }
        public override IHtmlString RenderItemColumn(System.Web.Mvc.ViewContext viewContext)
        {
            var inheritableData = DataItem as IInheritable;
            if (inheritableData != null && !inheritableData.IsLocalized(Site.Current))
            {
                var page = inheritableData as Page;

                if (page != null)
                {
                    if (page.Parent != null && page.Parent.IsLocalized(Site.Current))
                    {
                        return new HtmlString("Unsynced".Localize());
                    }
                }
                return new HtmlString("Inherited".Localize());
            }
            return new HtmlString("-");
        }
    }
}