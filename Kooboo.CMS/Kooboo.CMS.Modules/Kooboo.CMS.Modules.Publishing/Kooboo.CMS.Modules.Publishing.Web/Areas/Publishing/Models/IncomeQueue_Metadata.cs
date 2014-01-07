using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models
{
    [MetadataFor(typeof(IncomingQueue))]
    [Grid(IdProperty = "UUID", Checkable = true)]
    public class IncomeQueue_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "Vendor", GridColumnType = typeof(SortableGridColumn))]
        public string Vendor { get; set; }

        //[GridColumn(Order = 2, HeaderText = "Type", GridColumnType = typeof(SortableGridColumn))]
        //public PublishingObject PublishingObject { get; set; }

        [GridColumn(Order = 3, HeaderText = "Title",GridColumnType=typeof(SortableGridColumn),GridItemColumnType=typeof(TooltipGridItemColumn))]
        public string ObjectTitle { get; set; }

        //[GridColumn(Order = 4, HeaderText = "Action", GridColumnType = typeof(SortableGridColumn))]
        //public PublishingAction Action { get; set; }

        [GridColumn(Order = 5, HeaderText = "Creation date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2.ShortDateTimeGridItemColumnType))]
        public DateTime UtcCreationDate { get; set; }

        [GridColumn(Order = 6, HeaderText = "Status", GridColumnType = typeof(SortableGridColumn))]
        public QueueStatus Status { get; set; }

        [GridColumn(Order = 8, HeaderText = "Message", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(TooltipGridItemColumn))]
        public string Message { get; set; }
    }
}