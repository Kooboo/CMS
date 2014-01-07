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
    [MetadataFor(typeof(OutgoingQueue))]
    [Grid(IdProperty = "UUID", Checkable = true)]
    [GridColumn(HeaderText="Logs",Order=9,GridItemColumnType=typeof(OutgoingLogLinkGridItemColumn))]
    public class OutgoingQueue_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "Publishing object",GridColumnType=typeof(SortableGridColumn),GridItemColumnType=typeof(EditGridActionItemColumn))]
        [DisplayName("Publishing object")]
        [Required(ErrorMessage = "Required")]
        public PublishingObject PublishingObject { get; set; }

        [GridColumn(Order = 2, HeaderText = "Object UUID", GridColumnType = typeof(SortableGridColumn),GridItemColumnType=typeof(TooltipGridItemColumn))]
        public string ObjectUUID { get; set; }

        [GridColumn(Order = 3, HeaderText = "Remote endpoint", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Remote endpoint")]
        public string RemoteEndpoint { get; set; }

        [GridColumn(Order = 4, HeaderText = "Remote folder", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Remote folder")]
        public string RemoteFolderId { get; set; }

        [GridColumn(Order = 5, HeaderText = "Action", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Action")]
        public PublishingAction Action { get; set; }

        [GridColumn(Order = 6, HeaderText = "Retry times", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Retry times")]
        public int RetryTimes { get; set; }

        [GridColumn(Order = 7, HeaderText = "Last executed time", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        [DisplayName("Last executed time")]
        public DateTime? UtcLastExecutedTime { get; set; }

        [GridColumn(Order = 8, HeaderText = "Status", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Status")]
        [UIHint("DropDownList")]
        [DataSource(typeof(QueueStatusDataSource))]
        public QueueStatus Status { get; set; }

        [GridColumn(Order = 9, HeaderText = "Creation date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn))]
        [DisplayName("Creation date")]
        public DateTime UtcCreationDate { get; set; }
    }
}