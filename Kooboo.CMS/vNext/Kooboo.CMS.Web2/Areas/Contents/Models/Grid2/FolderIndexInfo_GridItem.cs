#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Search.Models;
using Kooboo.Common.Web.Grid;
using Kooboo.Common.Web.Grid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web2.Areas.Contents.Models.Grid2
{
    public class FolderIndexInfo_GridItem : GridItem
    {
        public FolderIndexInfo_GridItem(IGridModel gridModel, object dataItem, int dataIndex)
            : base(gridModel, dataItem, dataIndex)
        {
        }
        public override bool IsCheckable
        {
            get
            {
                var folderIndexInfo = (FolderIndexInfo)DataItem;
                return !folderIndexInfo.Rebuilding;
            }
        }
        public override IHtmlString RenderItemContainerAtts()
        {
            var folderIndexInfo = (FolderIndexInfo)DataItem;
            if (folderIndexInfo.Rebuilding)
            {
                return new HtmlString("class='rebuilding'");
            }
            else
            {
                return new HtmlString("");
            }
        }
    }
}