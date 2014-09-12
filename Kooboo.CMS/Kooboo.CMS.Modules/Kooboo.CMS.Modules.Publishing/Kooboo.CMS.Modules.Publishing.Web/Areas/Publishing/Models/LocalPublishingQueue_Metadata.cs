using Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.DataSources;
using Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Kooboo.CMS.Modules.Publishing.Models;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models
{
    [MetadataFor(typeof(LocalPublishingQueue))]
    [Grid(IdProperty = "UUID", Checkable = true)]
    public class LocalPublishingQueue_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "Type", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2.DetailGridItemColumn))]
        [DisplayName("Type")]
        [UIHint("DropDownList")]
        [DataSource(typeof(PublishingObjectDataSource))]
        [Required(ErrorMessage = "Required")]
        public PublishingObject PublishingObject { get; set; }

        [DisplayName("Object uuid")]
        public string ObjectUUID { get; set; }

        [GridColumn(Order = 2, HeaderText = "Title", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(TooltipGridItemColumn))]
        [DisplayName("Title")]
        public string ObjectTitle { get; set; }

        [GridColumn(Order = 3, HeaderText = "Time to publish", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        [UIHint("DateTime")]
        [DisplayName("Time to publish")]
        public DateTime? UtcTimeToPublish { get; set; }

        [GridColumn(Order = 4, HeaderText = "Time to unpublish", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        [UIHint("DateTime")]
        [DisplayName("Time to unpublish")]
        public DateTime? UtcTimeToUnpublish { get; set; }

        [GridColumn(Order = 5, HeaderText = "Published time", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        [DisplayName("Published time")]
        public DateTime? UtcProcessedPublishTime { get; set; }

        [GridColumn(Order = 6, HeaderText = "Unpublished time", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        [DisplayName("Unpublished time")]
        public DateTime? UtcProcessedUnpublishTime { get; set; }

        //[GridColumn(Order = 5, HeaderText = "Draft version", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        [DisplayName("Draft version")]
        public bool PublishDraft { get; set; }

        [GridColumn(Order = 7, HeaderText = "Creation date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2.ShortDateTimeGridItemColumnType))]
        [DisplayName("Creation date")]
        public DateTime UtcCreationDate { get; set; }

        //[GridColumn(Order = 7, HeaderText = "Processed publish time", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2.ShortDateTimeGridItemColumnType))]
        //public DateTime? UtcProcessedPublishTime { get; set; }

        //[GridColumn(Order = 7, HeaderText = "Processed unpublish time", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2.ShortDateTimeGridItemColumnType))]
        //public DateTime? UtcProcessedUnpublishTime { get; set; }


        [GridColumn(Order = 8, HeaderText = "User", GridColumnType = typeof(SortableGridColumn))]
        public string UserId { get; set; }

        [GridColumn(Order = 9, HeaderText = "Status", GridColumnType = typeof(SortableGridColumn))]
        public QueueStatus Status { get; set; }

        [GridColumn(Order = 10, HeaderText = "Message", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(TooltipGridItemColumn))]
        public string Message { get; set; }
    }
}