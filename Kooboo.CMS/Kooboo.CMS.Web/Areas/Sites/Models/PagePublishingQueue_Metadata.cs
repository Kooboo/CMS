#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(PagePublishingQueueItem))]
    [Grid(Checkable = true, IdProperty = "PageName")]
    public class PagePublishingQueue_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "Page name", GridColumnType = typeof(SortableGridColumn))]
        public string PageName { get; set; }

        [GridColumn(Order = 2, HeaderText = "Publish draft", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        public bool PublishDraft { get; set; }

        [GridColumn(Order = 3, HeaderText = "Creation date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        public DateTime CreationUtcDate { get; set; }

        [GridColumn(Order = 5, HeaderText = "Date to publish", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        public DateTime UtcDateToPublish { get; set; }

        [GridColumn(Order = 6, HeaderText = "Period", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        public bool Period { get; set; }

        [GridColumn(Order = 7, HeaderText = "Date to offline", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        public DateTime UtcDateToOffline { get; set; }

        [GridColumn(Order = 8)]
        public string UserName { get; set; }

        [GridColumn(Order = 9)]
        public Site Site
        {
            get;
            set;
        }

    }
}