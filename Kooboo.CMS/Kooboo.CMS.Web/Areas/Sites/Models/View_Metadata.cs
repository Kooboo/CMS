using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Sites.Models;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites.Services;

namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    public class ViewDataRuleRouteValueGetter : IGridActionRouteValuesGetter
    {
        public System.Web.Routing.RouteValueDictionary GetValues(object dataItem, System.Web.Routing.RouteValueDictionary routeValueDictionary, ViewContext viewContext)
        {
            routeValueDictionary["from"] = "view";
            return routeValueDictionary;
        }
    }
    //[GridAction(ActionName = "Index", ControllerName = "DataRule", DisplayName = "DataRules", Order = 0, RouteValueProperties = "fromName=Name", RouteValuesGetter = typeof(ViewDataRuleRouteValueGetter), VisibleArbiter = typeof(InheritableGridActionVisibleArbiter))]

    //[GridAction(ActionName = "Export", RouteValueProperties = "Name", Order = 3, VisibleArbiter = typeof(InheritableGridActionVisibleArbiter))]

    [GridAction(ActionName = "Copy", Class = "common-copy o-icon copy", Order = 3, RouteValueProperties = "sourceName=Name")]
    [GridAction(ActionName = "Relations", RouteValueProperties = "Name", Order = 6, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), Class = "o-icon relation dialog-link dialog-link", Title = "Relations")]
    [GridAction(DisplayName = "Localize", ActionName = "Localize", ConfirmMessage = "Are you sure you want to localize this item?",
        RouteValueProperties = "Name", Order = 7, Class = "o-icon localize", Renderer = typeof(LocalizationRender))]    
    [GridAction(ActionName = "Edit", RouteValueProperties = "Name", Order =8, CellVisibleArbiter = typeof(InheritableGridActionVisibleArbiter), Class = "o-icon edit")]
    [GridAction(DisplayName = "Version", ActionName = "Version", RouteValueProperties = "Name", Order = 9, ColumnVisibleArbiter = typeof(VersionGridActionVisibleArbiter), Class = "o-icon version dialog-link")]
    //[GridAction(ActionName = "Delete", ConfirmMessage = "Are you sure you want to delete this item?", RouteValueProperties = "Name", Order = 10, VisibleArbiter = typeof(InheritableGridActionVisibleArbiter))]
    [Grid(Checkable = true, IdProperty = "Name", CheckVisible = typeof(InheritableCheckVisible))]
    public class View_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [Remote("IsNameAvailable", "view", AdditionalFields = "SiteName,old_Key")]
        [GridColumn(ItemRenderType = typeof(CommonLinkNameColumnRender))]
        public string Name { get; set; }

        [UIHintAttribute("TemplateEditor")]
        public string Body { get; set; }

        [GridColumn(Order = 8)]
        public Site Site { get; set; }

        [UIHint("Plugins")]
        [DataSource(typeof(PluginDataSource))]
        public List<string> Plugins { get; set; }

        [UIHint("Parameters")]
        public List<Parameter> Parameters { get; set; }
    }

    public static class ViewDataSource
    {
        public static IEnumerable<SelectListItem> ForDropDownList(string currentValue)
        {
            return Kooboo.CMS.Sites.Services.ServiceFactory
                .ViewManager.All(Site.Current, "")
                .Select(o =>
                    new SelectListItem { Text = o.Name, Value = o.Name, Selected = o.Name == currentValue });
        }

        public static Kooboo.CMS.Sites.Services.Namespace<Kooboo.CMS.Sites.Models.View> GetNamespace(params string[] exculdes)
        {
            return Kooboo.CMS.Sites.Services.ServiceFactory
                .ViewManager.GetNamespace(Site.Current, exculdes);
        }

    }

    public class ViewGridModel
    {
        public Namespace<View> NameSpace { get; set; }
        public IEnumerable<View> ViewList { get; set; }
    }
}