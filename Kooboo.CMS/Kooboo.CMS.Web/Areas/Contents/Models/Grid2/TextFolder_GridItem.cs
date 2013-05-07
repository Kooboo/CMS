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

namespace Kooboo.CMS.Web.Areas.Contents.Models.Grid2
{
    public class TextFolder_GridItem : GridItem
    {
        public TextFolder_GridItem(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {
        }
        public override IHtmlString RenderItemContainerAtts()
        {
            var textFolder = (TextFolder)this.DataItem;
            if (textFolder.Hidden != null && textFolder.Hidden == true)
            {
                return new HtmlString("class='hidden-folder'");
            }
            return new HtmlString("");
        }

    }
}