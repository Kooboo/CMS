#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Sites.Models;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Web.Routing;
using Kooboo.ComponentModel;
using Kooboo.CMS.Web.Areas.Sites.Models.DataSources;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class PagePositionActionRouteValuesGetter : IGridActionRouteValuesGetter
    {
        #region IGridActionRouteValuesGetter Members

        public System.Web.Routing.RouteValueDictionary GetValues(object dataItem, System.Web.Routing.RouteValueDictionary routeValueDictionary, ViewContext viewContext)
        {
            routeValueDictionary["fullName"] = viewContext.RequestContext.GetRequestValue("fullName");

            return routeValueDictionary;
        }

        #endregion
    }

    public class PositionDescription : IItemColumnRender
    {
        #region IItemColumnRender Members

        public IHtmlString Render(object dataItem, object value, ViewContext viewContext)
        {
            return new HtmlString(dataItem.ToString());
        }

        #endregion
    }

    [MetadataFor(typeof(PagePosition))]
    [GridAction(ActionName = "EditPosition", DisplayName = "Edit", Order = 0, RouteValueProperties = "PagePositionId", RouteValuesGetter = typeof(PagePositionActionRouteValuesGetter))]
    [GridAction(ActionName = "DeletePosition", ConfirmMessage = "Are you sure you want to delete this item?", DisplayName = "Delete", Order = 5, RouteValueProperties = "PagePositionId", RouteValuesGetter = typeof(PagePositionActionRouteValuesGetter))]
    [GridColumn(HeaderText = "Description", Order = 2, ItemRenderType = typeof(PositionDescription))]
    public class PagePosition_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [GridColumn(Order = 0)]
        public string PagePositionId { get; set; }

        [GridColumn(Order = 1)]
        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(LayoutPositionsDataSource))]
        public string LayoutPositionId { get; set; }
        //[GridColumn(Order = 2)]
        //[UIHintAttribute("DropDownList")]
        //[DataSource(typeof(ViewNameList))]
        //public string ViewName { get; set; }
        [GridColumn(Order = 3)]
        public int Order { get; set; }
    }

    [MetadataFor(typeof(ViewPosition))]
    public class ViewPosition_Metadata : PagePosition_Metadata
    {
        [Required(ErrorMessage = "Required")]
        public string ViewName { get; set; }
    }

    [MetadataFor(typeof(ModulePosition))]
    public class ModulePosition_Metadata : PagePosition_Metadata
    {

        public string ModuleName { get; set; }

        public bool Exclusive { get; set; }
    }
    [MetadataFor(typeof(HtmlPosition))]
    public class HtmlPosition_Metadata : PagePosition_Metadata
    {
        public string Html { get; set; }
    }

    public class ModuleSettings_Metadata
    {
        public string ThemeName { get; set; }
    }
}