using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Content.Models;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Content.Versioning;

using Kooboo.Web.Mvc;
using Kooboo.CMS.Web.Models;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{

    public class VersionInfoActionGetter : IGridActionRouteValuesGetter
    {
        public System.Web.Routing.RouteValueDictionary GetValues(object dataItem, System.Web.Routing.RouteValueDictionary routeValueDictionary, System.Web.Mvc.ViewContext viewContext)
        {
            var data = (VersionInfo)dataItem;

            viewContext.RouteData.Values["UUID"] = data.TextContent["UUID"];

            return viewContext.RequestContext.AllRouteValues();
        }
    }
    public class RevertToVisible : IColumnVisibleArbiter
    {
        public bool IsVisible(System.Web.Mvc.ViewContext viewContext)
        {
            var folderName = viewContext.RequestContext.GetRequestValue("FolderName");
            var textFolder = new TextFolder(Repository.Current, folderName).AsActual();
            return Kooboo.CMS.Content.Services.ServiceFactory.WorkflowManager.AvailableToPublish(textFolder, viewContext.HttpContext.User.Identity.Name);
        }
    }

    [GridAction(ActionName = "RevertTo", DisplayName = "Revert", ConfirmMessage = "Are you sure you want to revert to this version?", RouteValueProperties = "Version", Class = "o-icon revert", ColumnVisibleArbiter = typeof(RevertToVisible))]
    [Grid(Checkable = true, IdProperty = "Version")]
    public class VersionInfo_Metadata
    {
        [GridColumn]
        public int Version { get; set; }

        [GridColumn(HeaderText = "Commit user")]
        public string CommitUser { get; set; }

        [GridColumn(HeaderText = "Commit date", ItemRenderType = typeof(DateTimeColumnRender))]
        public DateTime UtcCommitDate { get; set; }

    }
}