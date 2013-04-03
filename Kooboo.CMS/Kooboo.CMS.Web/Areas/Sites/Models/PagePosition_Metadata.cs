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
        [DataSource(typeof(LayoutPositionList))]
        public string LayoutPositionId { get; set; }
        //[GridColumn(Order = 2)]
        //[UIHintAttribute("DropDownList")]
        //[DataSource(typeof(ViewNameList))]
        //public string ViewName { get; set; }
        [GridColumn(Order = 3)]
        public int Order { get; set; }
    }

    public class LayoutPositionList : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var site = Site.Current;

            var fullName = requestContext.GetRequestValue("fullName");
            var page = Kooboo.CMS.Sites.Services.ServiceFactory.PageManager.Get(site, fullName);
            Layout layout = Kooboo.CMS.Sites.Services.ServiceFactory.LayoutManager.Get(site, page.Layout);

            var positionList = layout.Positions;
            foreach (var position in positionList)
            {
                yield return new System.Web.Mvc.SelectListItem() { Value = position.ID, Text = position.ID };
            }
        }
    }

    public class ViewNameList : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var site = Site.Current;

            var viewNameList = Kooboo.CMS.Sites.Persistence.Providers.ViewProvider.All(site);
            foreach (var l in viewNameList)
            {
                yield return new System.Web.Mvc.SelectListItem() { Value = l.Name, Text = l.Name };
            }

        }
    }


    public class ViewPosition_Metadata : PagePosition_Metadata
    {
        [Required(ErrorMessage = "Required")]
        public string ViewName { get; set; }
    }

    public class ModulePosition_Metadata : PagePosition_Metadata
    {

        public string ModuleName { get; set; }

        public bool Exclusive { get; set; }
    }
    public class HtmlPosition_Metadata : PagePosition_Metadata
    {
        public string Html { get; set; }
    }

    public class ModuleSettings_Metadata
    {
        public string ThemeName { get; set; }
    }
}