using Kooboo.CMS.Modules.Publishing.Models;
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

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models
{
    [Grid(IdProperty="UUID",Checkable=true)]
    [MetadataFor(typeof(RemoteTextFolderMapping))]
    public class RemoteTextFolderMapping_Metadata
    {
        [GridColumn(Order = 1, HeaderText = "Name",GridColumnType=typeof(SortableGridColumn), GridItemColumnType = typeof(EditGridActionItemColumn))]
        [Required(ErrorMessage = "Required")]
        public string Name { get; set; }

        [GridColumn(Order = 2, HeaderText = "Local folder", GridColumnType = typeof(SortableGridColumn),GridItemColumnType=typeof(TooltipGridItemColumn))]
        [DisplayName("Local folder")]
        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownList")]
        [DataSource(typeof(TextFoldersDataSource))]
        public string LocalFolderId { get; set; }

        [GridColumn(Order = 3, HeaderText = "Remote site", GridColumnType = typeof(SortableGridColumn))]
        [DisplayName("Remote site")]
        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownList")]
        [DataSource(typeof(RemoteEndpointSettingDataSource))]
        public string RemoteEndpoint { get; set; }

        [GridColumn(Order = 4, HeaderText = "Remote folder", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(TooltipGridItemColumn))]
        [DisplayName("Remote folder")]
        [Required(ErrorMessage = "Required")]
        [UIHint("DropDownList")]
        public string RemoteFolderId { get; set; }

        [GridColumn(Order = 5, HeaderText = "Enabled", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(BooleanGridItemColumn))]
        [DisplayName("Enabled")]
        public bool Enabled { get; set; }

    }
}