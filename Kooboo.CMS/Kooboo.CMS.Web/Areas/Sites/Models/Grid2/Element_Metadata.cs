#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.ComponentModel.DataAnnotations;

using Kooboo.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.ComponentModel;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;
using Kooboo.Web.Mvc.Grid2.Design;
using Kooboo.CMS.Web.Areas.Sites.Models.Grid2;
using Kooboo.Web.Mvc.Grid2;
using System.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class Element_Name_GridItemColumn : GridItemColumn
    {
        public Element_Name_GridItemColumn(IGridColumn gridColumn, object dataItem, object propertyValue)
            : base(gridColumn, dataItem, propertyValue)
        {
        }
        public override System.Web.IHtmlString RenderItemColumnContainerAtts(System.Web.Mvc.ViewContext viewContext)
        {
            return new HtmlString("class='common'");
        }
    }
    public class Element_Name_GridColumn : GridColumn
    {
        public Element_Name_GridColumn(GridModel gridModel, GridColumnAttribute att, string propertyName, int order)
            : base(gridModel, att, propertyName, order)
        {

        }
        public override IHtmlString RenderHeaderContainerAtts(System.Web.Mvc.ViewContext viewContext)
        {
            return new HtmlString("class='common'");
        }
    }
    [Grid(Checkable = true, IdProperty = "Name")]
    [MetadataFor(typeof(Element))]
    public class Element_Metadata
    {
        [GridColumn(Order = 1, GridColumnType = typeof(Element_Name_GridColumn), GridItemColumnType = typeof(Element_Name_GridItemColumn))]
        [Required(ErrorMessage = "Required")]
        public string Name
        {
            get;
            set;
        }

        [UIHint("AutoComplete")]
        [DataSource(typeof(ElementCategoryCulturesSelectListDataSource))]
        public string Category
        {
            get;
            set;
        }

        [GridColumn(GridItemColumnType = typeof(Element_Value_GridItemColumn), Order = 2)]
        [UIHint("MultilineText")]
        public string Value
        {
            get;
            set;
        }
    }
}