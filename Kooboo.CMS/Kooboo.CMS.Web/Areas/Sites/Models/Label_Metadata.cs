using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
using Kooboo.CMS.Web.Grid2;
using Kooboo.Common.ComponentModel;
using Kooboo.Common.Web.Grid.Design;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [Grid(Checkable = true, IdProperty = "UUID")]
    [MetadataFor(typeof(Label))]
    public class Label_Metadata
    {
        public string UUID
        {
            get;
            set;
        }
        [GridColumn(Order = 1, GridColumnType = typeof(SortableGridColumn))]
        public string Name { get; set; }

        public string Category { get; set; }

        [GridColumn(HeaderText = "Value", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(Label_Value_GridItemColumn), Order = 3)]
        public string Value { get; set; }

        [GridColumn(HeaderText = "Creation date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn), Order = 4)]
        public DateTime? UtcCreationDate { get; set; }

        [GridColumn(HeaderText = "Lastest modification date", GridColumnType = typeof(SortableGridColumn), GridItemColumnType = typeof(DateTimeGridItemColumn), Order = 5)]
        public DateTime? UtcLastestModificationDate { get; set; }

        [GridColumn(HeaderText = "Editor", GridColumnType = typeof(SortableGridColumn), Order = 6)]
        public string LastestEditor { get; set; }
    }
}