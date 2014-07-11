#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.Common.Web.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class PageGridItem : InheritablGridItem
    {
        public PageGridItem(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {
        }
        public override IHtmlString RenderItemContainerAtts()
        {
            var @class = base.RenderItemContainerAtts();

            return new HtmlString(@class + "data-pagename='" + ((Page)this.DataItem).FullName + "'");
        }
        protected override string GetCssClass()
        {
            var @class = base.GetCssClass();
            var page = (Page)this.DataItem;
            if (page.Published.HasValue && page.Published.Value == true)
            {
                @class += " published";
            }
            else
            {
                @class += " unpublished";
            }
            if (page.IsDefault)
            {
                @class += " homepage";
            }
            return @class;
        }
    }
}