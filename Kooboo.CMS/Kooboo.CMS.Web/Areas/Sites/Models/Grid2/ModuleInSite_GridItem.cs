#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Web.Mvc.Grid2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models.Grid2
{
    public class ModuleInSite_GridItem : GridItem
    {
        public ModuleInSite_GridItem(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {
        }
        public override IHtmlString RenderItemContainerAtts()
        {
            var data = (ModuleListInSiteModel)this.DataItem;
            var @class = "";
            if (data.Included)
            {
                @class = "included";
            }
            else
            {
                @class = "excluded";
            }
            return new HtmlString("class='" + @class + "'");
        }
    }
}