using Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Modules.Publishing.Models;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models
{
    [MetadataFor(typeof(PublishingLog))]
    [Grid(IdProperty = "UUID")]
    public class PublishingLog_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "Publishing type", GridColumnType = typeof(SortableGridColumn))]
        public QueueType QueueType { get; set; }

        [GridColumn(Order = 2, HeaderText = "Action", GridColumnType = typeof(SortableGridColumn))]
        public PublishingAction PublishingAction { get; set; }

        [GridColumn(Order = 3, HeaderText = "Item type", GridColumnType = typeof(SortableGridColumn))]
        public PublishingObject PublishingObject { get; set; }


        [GridColumn(Order = 4, HeaderText = "Title", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(TooltipGridItemColumn))]
        public string ObjectTitle { get; set; }

        //[GridColumn(Order = 4, HeaderText = "Publishing type", GridColumnType = typeof(SortableGridColumn))]
        //public PublishingType PublishingType { get; set; }



        [GridColumn(Order = 6, HeaderText = "Remote sites", GridColumnType = typeof(SortableGridColumn))]
        //Use for publishing page
        public string RemoteEndpoint { get; set; }

        [GridColumn(Order = 7, HeaderText = "Folder mapping", GridColumnType = typeof(SortableGridColumn))]
        //Use for publishing text content.
        public string TextFolderMapping { get; set; }

        [GridColumn(Order = 8, HeaderText = "User", GridColumnType = typeof(SortableGridColumn))]
        public string UserId { get; set; }


        [GridColumn(Order = 9, HeaderText = "Draft version", GridColumnType = typeof(SortableGridColumn))]
        public bool PublishDraft { get; set; }

        [GridColumn(Order = 10, HeaderText = "Status", GridColumnType = typeof(SortableGridColumn))]
        public QueueStatus Status { get; set; }

        [GridColumn(Order = 11, HeaderText = "Processed time", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        public DateTime? UtcProcessedTime { get; set; }

        [GridColumn(Order = 12, HeaderText = "Message", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(TooltipGridItemColumn))]
        public string Message { get; set; }

        [GridColumn(Order = 13, HeaderText = "Stack trace", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(TooltipGridItemColumn))]
        public string StackTrace { get; set; }
    }
}