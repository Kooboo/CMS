using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc.Grid;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.Extensions;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.Globalization;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{

    public class ElementValueRender : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, ViewContext viewContext)
        {
            var site = viewContext.RequestContext.GetRequestValue("siteName");
            UrlHelper urlHelper = new UrlHelper(viewContext.RequestContext);
            Element element = (Element)dataItem;

            var url = urlHelper.Action("Edit", new { siteName = site, name = element.Name, category = element.Category });
            var renderStr = string.Format(@"<span class=""left"">{0}</span><a class=""o-icon edit inline-action"" href=""{1}"">Edit</a>", HttpUtility.HtmlEncode(element.Value), url);

            return new HtmlString(renderStr);
        }
    }


    public class ElementCategoryCulturesSelectListDataSource : ISelectListDataSource
    {

        #region ISelectListDataSource Members

        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var site = SiteHelper.Parse(requestContext.GetRequestValue("siteName"));
            var categories = Kooboo.CMS.Sites.Services.ServiceFactory.LabelManager.GetCategories(site).Where(it => !string.IsNullOrEmpty(it.Category));
            if (!string.IsNullOrEmpty(filter))
            {
                categories = categories.Where(it => it.Category.StartsWith(filter, StringComparison.CurrentCultureIgnoreCase));
            }
            return categories.Select(it => new SelectListItem() { Text = it.Category, Value = it.Category });
        }

        #endregion
    }

    public class ElementActionRouteValues : IGridActionRouteValuesGetter
    {
        #region IGridActionRouteValuesGetter Members

        public System.Web.Routing.RouteValueDictionary GetValues(object dataItem, System.Web.Routing.RouteValueDictionary routeValueDictionary, System.Web.Mvc.ViewContext viewContext)
        {
            routeValueDictionary["page"] = viewContext.RequestContext.GetRequestValue("page");
            routeValueDictionary["pageSize"] = viewContext.RequestContext.GetRequestValue("pageSize");
            return routeValueDictionary;
        }

        #endregion
    }

    //[GridAction(ActionName = "Edit", RouteValueProperties = "Category,Name,Culture", Order = 1, RouteValuesGetter = typeof(ElementActionRouteValues), Icon = "Edit.png")]
    //[GridAction(ActionName = "Translate", RouteValueProperties = "Category,Name,Culture", Order = 1, RouteValuesGetter = typeof(ElementActionRouteValues))]
    //[GridAction(ActionName = "Delete", ConfirmMessage = "Are you sure you want to delete this item?", RouteValueProperties = "Category,Name,Culture", Order = 8, RouteValuesGetter = typeof(ElementActionRouteValues))]
    [Grid(Checkable = true, IdProperty = "Name")]
    public class Element_Metadata
    {


        [GridColumn(Order = 1)]
        [Required(ErrorMessage = "Required")]
        public string Name
        {
            get;
            set;
        }

        //[GridColumn(Order = 3)]
        [UIHint("AutoComplete")]
        [DataSource(typeof(ElementCategoryCulturesSelectListDataSource))]
        public string Category
        {
            get;
            set;
        }
        //[GridColumn(Order = 5)]
        //[UIHintAttribute("DropDownList")]
        //[DataSource(typeof(CultureSelectListDataSource))]
        //public string Culture
        //{
        //    get;
        //    set;
        //}



        [GridColumn(Order = 8, ItemRenderType = typeof(ElementValueRender))]
        [UIHint("MultilineText")]
        public string Value
        {
            get;
            set;
        }
    }
}