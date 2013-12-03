using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Web.Grid2;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models
{
    [Grid(IdProperty="UUID",Checkable=true)]
    [MetadataFor(typeof(RemoteEndpointSetting))]
    public class RemoteEndpointSetting_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "Name",GridColumnType=typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [GridColumn(Order = 2, HeaderText = "Cmis service", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Cmis service")]
        [Description("eg.: http://www.mysite.com/api/cmis.svc")]
        [Required(ErrorMessage = "Required")]
        public string CmisService { get; set; }

        [GridColumn(Order = 3, HeaderText = "Cmis user name", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Cmis user name")]
        [Required(ErrorMessage = "Required")]
        public string CmisUserName { get; set; }
                
        [DisplayName("Cmis password")]
        [UIHint("Password")]
        [Required(ErrorMessage = "Required")]
        public string CmisPassword { get; set; }

        [GridColumn(Order = 5, HeaderText = "Max retry times", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Max retry times")]
        [UIHint("Spinner")]
        [Required(ErrorMessage = "Required")]
        public int MaxRetryTimes { get; set; }

        [GridColumn(Order = 6, HeaderText = "Remote site", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Remote site")]
        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownList")]
        [Description("You can reget remote site list when the refresh button is lighten.")]
        public string RemoteRepositoryId { get; set; }

        //[GridColumn(Order = 7, HeaderText = "Publish page automatically", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        //[DisplayName("Publish page automatically")]
        //public bool PublishPageAutomatically { get; set; }

        [GridColumn(Order = 8, HeaderText = "Enabled", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        [DisplayName("Enabled")]
        public bool Enabled { get; set; }

    }
}