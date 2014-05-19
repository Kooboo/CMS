using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.CMS.Web.Models;
using Kooboo.ComponentModel;
using Kooboo.Web.Mvc.Grid2.Design;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(DataSourceSetting))]
    [Grid(Checkable = true, IdProperty = "UUID", GridItemType = typeof(InheritablGridItem), EmptyText = "No datasources")]
    [GridColumn(GridItemColumnType = typeof(Inheritable_Status_GridItemColumn), HeaderText = "Inheritance", Order = 2)]
    public class DataSourceSetting_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.Alphanum, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [RemoteEx("IsDataNameAvailable", "DataSource", RouteFields = "SiteName")]
        public string DataName { get; set; }

        public IDataSource DataSource { get; set; }

        [GridColumn(Order = 1, HeaderText = "Data name", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(InheritableEditGridActionItemColumn))]
        public string UUID
        {
            get;
            set;
        }
    }
}