using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.Web.Mvc.Grid;
using Kooboo.CMS.Content.Models;
using System.ComponentModel.DataAnnotations;
using Kooboo.CMS.Web.Models;
using Kooboo.Web.Mvc;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    [Grid(Checkable = true, IdProperty = "Name")]
    [GridAction(ActionName = "Edit", Class = "o-icon edit dialog-link", Order = 0, RouteValueProperties = "Name")]
    public class Workflow_Metadata
    {
        [GridColumn(ItemRenderType = typeof(CommonLinkPopColumnRender))]
        [Required]
        public string Name { get; set; }

        [UIHint("WorkflowItems")]
        public WorkflowItem[] Items { get; set; }
    }

    public class WorkflowDataSource : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(System.Web.Routing.RequestContext requestContext, string filter = null)
        {
            return Kooboo.CMS.Content.Services.ServiceFactory
                .WorkflowManager
                .All(Repository.Current)
                .Select(o => new SelectListItem
                {
                    Value = o.Name,
                    Text = o.Name
                });
        }
    }

    [Grid(Checkable = false, IdProperty = "Id")]
    [GridAction(ActionName = "Edit", ControllerName = "TextContent", Class = "o-icon edit dialog-link", Order = 0, RouteValueProperties = "FolderName=ContentFolder,UUID=ContentUUID", DisplayName = "Edit content")]
    [GridAction(ActionName = "Process", Class = "o-icon process dialog-link", Order = 0, RouteValueProperties = "Id,WorkflowName", DisplayName = "Process")]
    public class PendingWorkflowItem_Metadata
    {
        [GridColumn(HeaderText = "Step name", Order = 0, ItemRenderType = typeof(PendingWorkflowItemPopLink))]
        [Display(Name = "Step name")]
        public string ItemDisplayName { get; set; }

        [GridColumn(Order = 2, HeaderText = "Workflow name")]
        [Display(Name = "Workflow name")]
        public string WorkflowName { get; set; }

        [Display(Name = "Workflow item sequence")]
        public string WorkflowItemSequence { get; set; }

        [Display(Name = "Role")]
        [GridColumn(Order = 4, HeaderText = "Role name")]
        public string RoleName { get; set; }

        //[Display(Name = "Folder name")]
        //[GridColumn(Order = 6, HeaderText = "Folder name")]
        public string ContentFolder { get; set; }

        public string ContentUUID { get; set; }

        //[Display(Name = "Content summary")]
        //[GridColumn(Order = 8, HeaderText = "Content summary")]
        public string ContentSummary { get; set; }

        [Display(Name = "Creation date")]
        public DateTime CreationUtcDate { get; set; }

        [Display(Name = "Creation user")]
        [GridColumn(Order = 12, HeaderText = "Creation user")]
        public string CreationUser { get; set; }


    }


    public class PendingWorkflowItemPopLink : CommonLinkPopColumnRender
    {
        public override string GetUrl(object dataItem, object value, ViewContext viewContext)
        {
            var data = (PendingWorkflowItem)dataItem;
            UrlHelper url = new UrlHelper(viewContext.RequestContext);
            return url.Action("Process", viewContext.RequestContext.AllRouteValues().Merge("Id", data.Name).Merge("WorkflowName", data.WorkflowName));
        }
    }

    public class WorkflowHistory_Metadata
    {
        [GridColumn(HeaderText = "Flow order",Order=1)]
        public int Id { get; set; }

        [GridColumn(HeaderText = "Workflow", Order = 2)]
        public string WorkflowName { get; set; }

        [GridColumn(HeaderText = "Sequence", Order = 3)]
        public string WorkflowItemSequence { get; set; }

        [GridColumn(HeaderText = "Step name", Order = 4)]
        public string ItemDisplayName { get; set; }

        [GridColumn(HeaderText = "Role", Order = 5)]
        public string RoleName { get; set; }

        public string ContentUUID { get; set; }

        [GridColumn(HeaderText = "Content summary",Order=6)]
        public string ContentSummary { get; set; }

        [GridColumn(ItemRenderType = typeof(BooleanColumnRender), HeaderText = "Passed", Order = 7)]
        public bool Passed { get; set; }

        [GridColumn(HeaderText = "Processer", Order = 8)]
        public string ProcessingUser { get; set; }

        [GridColumn(HeaderText = "Processed date", ItemRenderType = typeof(DateTimeColumnRender), Order = 9)]
        public DateTime ProcessingUtcDate { get; set; }

        [GridColumn(Order=10)]
        public string Comment { get; set; }

        [GridColumn(ItemRenderType = typeof(BooleanColumnRender), HeaderText = "Finished", Order = 11)]
        public bool Finished { get; set; }
    }
}