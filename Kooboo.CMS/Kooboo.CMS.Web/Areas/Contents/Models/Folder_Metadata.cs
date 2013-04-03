using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Web.Areas.Contents.Controllers;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Grid;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{

    public class TextFolderNameColumnItemRender : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, ViewContext viewContext)
        {
            var data = ((TextFolder)dataItem).AsActual();
            UrlHelper url = new UrlHelper(viewContext.RequestContext);
            if (!string.IsNullOrEmpty(data.SchemaName))
            {
                return new HtmlString(string.Format(@"<a  class='f-icon folder'  href=""{0}"" title=""{1}"">{1}</a>"
                , url.Action("Index", "TextContent", viewContext.RequestContext.AllRouteValues().Merge("FolderName", data.FullName).Merge("Folder", data.FullName).Merge("FullName", data.FullName))
                , data.FriendlyText));
            }
            else
            {
                return new HtmlString(string.Format(@"<a  class='f-icon folder'  href=""{0}"" title=""{1}"">{1}</a>"
                , url.Action("Index", "TextFolder", viewContext.RequestContext.AllRouteValues().Merge("FolderName", data.FullName).Merge("Folder", data.FullName).Merge("FullName", data.FullName))
                , data.FriendlyText));
            }


        }
    }


    public class FolderRouteValuesGetter : IGridActionRouteValuesGetter
    {
        public System.Web.Routing.RouteValueDictionary GetValues(object dataItem, System.Web.Routing.RouteValueDictionary routeValueDictionary, System.Web.Mvc.ViewContext viewContext)
        {
            var folder = ((Folder)dataItem).AsActual();
            //if (folder is ContainerFolder)
            //{
            //    routeValueDictionary["folderType"] = "Container";
            //}
            if (folder is TextFolder)
            {
                routeValueDictionary["folderType"] = "Text";
            }
            else if (folder is MediaFolder)
            {
                routeValueDictionary["folderType"] = "Binary";
            }
            return routeValueDictionary;
        }
    }
    //[GridAction(ActionName = "Export", ControllerName = "Folder", RouteValueProperties = "Name,FullName", RouteValuesGetter = typeof(FolderRouteValuesGetter), Order = 0)]
    [GridAction(ActionName = "Edit", Class = "o-icon edit", ControllerName = "Folder", RouteValueProperties = "Name,FullName", RouteValuesGetter = typeof(FolderRouteValuesGetter), Order = 1)]
    //[GridAction(ActionName = "Delete", ControllerName = "Folder", RouteValueProperties = "Name,FullName", RouteValuesGetter = typeof(FolderRouteValuesGetter), Order = 3, ConfirmMessage = "Are you sure you want to delete this item?")]
    [Grid(Checkable = true, IdProperty = "FullName")]
    public class Folder_Metadata
    {
        [GridColumn(ItemRenderType = typeof(TextFolderNameColumnItemRender))]
        public string Name { get; set; }


        [DisplayName("Display name")]
        public string DisplayName { get; set; }

        //public BroadcastSetting BroadcastSetting { get; set; }
    }



    public class ContainerFolder_Metadata
    {
        public string Name { get; set; }

        [DisplayName("Display name")]
        public string DisplayName { get; set; }
        //public BroadcastSetting BroadcastSetting { get; set; }
    }




    //public class BroadcastSetting_Metadata
    //{
    //    public bool EnableSending { get; set; }

    //    [UIHint("NullabledBoolean")]
    //    public bool? Published { get; set; }

    //    //public List<FieldValue> Filters { get; set; }

    //    [UIHint("DropDownCheckList")]
    //    [DataSource(typeof(AllRepository))]
    //    public List<string> ReceivingRepositories { get; set; }
    //}

    public class SchemaList : ISelectListDataSource
    {
        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            yield return new SelectListItem() { Text = "None".Localize(), Value = "" };
            var repository = Repository.Current;
            foreach (var item in ServiceFactory.SchemaManager.All(repository, ""))
            {
                yield return new SelectListItem() { Text = item.Name, Value = item.Name };
            }
        }
    }
    public class TextFolderEditable : IVisibleArbiter
    {
        public bool IsVisible(object dataItem, ViewContext viewContext)
        {
            return Kooboo.CMS.Web.Authorizations.AuthorizationHelpers.Authorize(viewContext.RequestContext, Kooboo.CMS.Account.Models.Permission.Contents_FolderPermission);
        }
    }

    [GridAction(ActionName = "Edit", RouteValueProperties = "Name,FullName", RouteValuesGetter = typeof(FolderRouteValuesGetter), Order = 1, Class = "o-icon edit dialog-link", Title = "Edit", CellVisibleArbiter = typeof(TextFolderEditable))]
    //[GridAction(ActionName = "Delete", ControllerName = "Folder", RouteValueProperties = "Name,FullName", RouteValuesGetter = typeof(FolderRouteValuesGetter), Order = 3, ConfirmMessage = "Are you sure you want to delete this item?")]
    [Grid(Checkable = true, IdProperty = "FullName")]
    public class TextFolder_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        [GridColumn(ItemRenderType = typeof(TextFolderNameColumnItemRender))]
        public string Name { get; set; }

        [DisplayName("Display name")]
        public string DisplayName { get; set; }
        //public BroadcastSetting BroadcastSetting { get; set; }

        [UIHint("DropDownList")]
        [DataSource(typeof(SchemaList))]
        [DisplayName("Content type")]
        public string SchemaName { get; set; }

        [Display(Name = "Category folders")]
        [UIHint("DropDownTreeArray")]
        [DataSource(typeof(FolderTreeDataSource))]
        public IEnumerable<string> CategoryFolders { get; set; }

        [Display(Name = "Category folders")]
        [UIHint("Categories")]
        [DataSource(typeof(FolderTreeDataSource))]
        public IEnumerable<CategoryFolder> Categories { get; set; }

        [Display(Name = "Embedded folders")]
        [Description("Embed content from another content folder. For example an <b>article</b> embeds <b>comments</b>")]
        [UIHint("DropDownTreeArray")]
        [DataSource(typeof(FolderTreeDataSource))]
        public string[] EmbeddedFolders { get; set; }

        [Display(Name = "Workflow name")]
        [UIHint("DropDownList")]
        [DataSource(typeof(WorkflowDataSource))]
        public string WorkflowName { get; set; }

        [UIHint("DropDownArray")]
        [DataSource(typeof(RolesDatasource))]
        public string[] Roles { get; set; }

        [Display(Name = "Enabled worflow")]
        public bool EnabledWorkflow { get; set; }

        [UIHint("OrderSetting")]
        public OrderSetting OrderSetting { get; set; }

        [DisplayName("Visible on sidebar menu")]
        [Description("Hide the folder from the menu in the left slide bar when it was unchecked.")]
        public bool? VisibleOnSidebarMenu
        {
            get;
            set;
        }
        [DisplayName("Page size")]
        [Description("The item count on management grid page.")]
        public int? PageSize { get; set; }
    }
    public class OrderDirections : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            yield return new SelectListItem() { Text = "Ascending", Value = "0" };
            yield return new SelectListItem() { Text = "Descending", Value = "1", Selected = true };
        }
    }
    public class OrderSetting_Metadata
    {
        [Display(Name = "Order field")]
        [UIHint("DropDownList")]
        public string FieldName { get; set; }
        [Display(Name = "Order direction")]
        [UIHint("DropDownList")]
        [DataSource(typeof(OrderDirections))]
        public OrderDirection Direction { get; set; }
    }


    public class AllRepository : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var allRepository = ServiceFactory.RepositoryManager.All();
            var repository = Repository.Current;
            foreach (var item in allRepository)
            {
                if (item.Name != repository.Name)
                    yield return new SelectListItem() { Text = item.Name, Value = item.Name };
            }
        }
    }




    public class MediaFolderNameColumnItemRender : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, ViewContext viewContext)
        {
            var data = (Folder)dataItem;
            return new HtmlString(string.Format("<a class='folder' href='/Contents/MediaContent/Index/?repositoryName={0}&FolderName={1}&FullName={1}'>{2}</a>", data.Repository.Name, data.FullName, string.IsNullOrEmpty(data.DisplayName) ? data.Name : data.DisplayName));
        }
    }

    [GridAction(ActionName = "Edit", Icon = "edit.png", RouteValueProperties = "Name,FullName", Order = 1)]
    [Grid(Checkable = true, IdProperty = "Name")]
    public class MediaFolder_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [GridColumn(ItemRenderType = typeof(MediaFolderNameColumnItemRender))]
        public string Name { get; set; }
        [DisplayName("Display name")]
        public string DisplayName { get; set; }
        //public BroadcastSetting BroadcastSetting { get; set; }
        [UIHint("AllowExtension")]
        public IEnumerable<string> AllowedExtensions { get; set; }
    }




    public class FolderDataSource : ISelectListDataSource
    {
        public FolderDataSource()
        {
            Repository = Repository.Current;
        }
        public FolderDataSource(Repository repository)
        {
            this.Repository = repository;
        }
        public FolderDataSource(string repository)
            : this(ServiceFactory.RepositoryManager.Get(repository))
        {

        }
        public Repository Repository { get; set; }
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            var repository = this.Repository;
            if (repository == null)
            {
                repository = Repository.Current;
            }
            return ServiceFactory.TextFolderManager.All(repository, "")
                .Select(o => new SelectListItem()
            {
                Text = string.IsNullOrEmpty(o.DisplayName) ? o.Name : o.DisplayName,
                Value = o.FullName
            });
        }

        public IEnumerable<SelectListItemTree> GetDatasourceTree(RequestContext requestContext)
        {
            var repository = Repository;
            var all = ServiceFactory.TextFolderManager.All(repository, "");
            if (all == null)
            {
                return null;
            }
            return all.Select(o => InitItem(o, repository));
        }
        private SelectListItemTree InitItem(TextFolder textFolder, Repository repository)
        {
            SelectListItemTree item = new SelectListItemTree
            {
                Text = string.IsNullOrEmpty(textFolder.DisplayName) ? textFolder.Name : textFolder.DisplayName,
                Value = textFolder.FullName
            };

            textFolder.Repository = repository;

            var childFolders = ServiceFactory.TextFolderManager.ChildFolders(textFolder);

            if (childFolders != null)
            {
                item.Items = childFolders.Select(o => InitItem(o, repository));
            }

            return item;
        }
    }


    public class FolderTreeDataSource : ISelectListDataSource
    {

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter = null)
        {
            return new FolderDataSource().GetDatasourceTree(requestContext);
        }
    }
}