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
    [MetadataFor(typeof(PublishingQueue))]
    [Grid(IdProperty="UUID",Checkable=true)]
    public class PublishingQueue_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "Publishing object",GridColumnType=typeof(SortableGridColumn), GridItemColumnType = typeof(Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models.Grid2.DetailGridItemColumn))]
        [DisplayName("Publishing object")]
        [UIHint("DropDownList")]
        [DataSource(typeof(PublishingObjectDataSource))]
        [Required(ErrorMessage = "Required")]
        public PublishingObject PublishingObject { get; set; }

        [GridColumn(Order = 2, HeaderText = "Object UUID", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(TooltipGridItemColumn))]
        public string ObjectUUID { get; set; }

        [GridColumn(Order = 3, HeaderText = "Publishing type", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Publishing type")]
        [UIHint("DropDownList")]
        [DataSource(typeof(PublishingTypeDataSource))]
        [Required(ErrorMessage = "Required")]
        public PublishingType PublishingType { get; set; }

        [GridColumn(Order = 4, HeaderText = "Publishing time", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(Kooboo.CMS.Web.Grid2.DateTimeGridItemColumn))]
        [UIHint("DateTime")]
        [DisplayName("Publishing time")]
        public DateTime? UtcTimeToPublish { get; set; }

        [GridColumn(Order = 5, HeaderText = "Unpublishing time", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(Kooboo.CMS.Web.Grid2.DateTimeGridItemColumn))]
        [UIHint("DateTime")]
        [DisplayName("Unpublishing time")]
        public DateTime? UtcTimeToUnpublish { get; set; }

        [GridColumn(Order = 6, HeaderText = "Remote endpoints", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(ArrayGridItemColumn))]
        [DisplayName("Remote endpoints")]
        [UIHint("Multiple_DropDownList")]
        [DataSource(typeof(RemoteEndpointSettingDataSource))]
        public string[] RemoteEndpoints { get; set; }

        [GridColumn(Order = 7, HeaderText = "Publishing mappings", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(ArrayGridItemColumn))]
        [DisplayName("Publishing mappings")]
        [UIHint("Multiple_DropDownList")]
        [DataSource(typeof(RemotePublishingMappingDataSource))]
        public string[] PublishingMappings { get; set; }

        [GridColumn(Order = 8, HeaderText = "Publishing draft", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        [DisplayName("Publishing draft")]
        public bool PublishDraft { get; set; }

        [GridColumn(Order = 9, HeaderText = "Creation date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(Kooboo.CMS.Web.Grid2.DateTimeGridItemColumn))]
        [DisplayName("Creation date")]
        public DateTime UtcCreationDate { get; set; }

        [GridColumn(Order = 10, HeaderText = "User", GridColumnType = typeof(SortableGridColumn))]
        public string UserId { get; set; }

        [GridColumn(Order = 11, HeaderText = "Status", GridColumnType = typeof(SortableGridColumn))]
        public QueueStatus Status { get; set; }

        [GridColumn(Order = 12, HeaderText = "Message", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(TooltipGridItemColumn))]
        public string Message { get; set; }
    }
}