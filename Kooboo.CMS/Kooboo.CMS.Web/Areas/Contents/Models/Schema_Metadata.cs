using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc.Grid;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Content.Persistence;
using Kooboo.CMS.Web.Areas.Contents.Controllers;
using System.Web.Mvc;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using System.ComponentModel;
using System.Web.Routing;
using Kooboo.CMS.Web.Models;

namespace Kooboo.CMS.Web.Areas.Contents.Models
{
    public class ChildSchemaDataSource : ISelectListDataSource
    {
        #region ISelectListDataSource Members

        public IEnumerable<System.Web.Mvc.SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var repository = Repository.Current;
            foreach (var item in ServiceFactory.SchemaManager.All(repository, ""))
            {
                yield return new SelectListItem() { Text = item.Name, Value = item.Name };
            }
        }

        #endregion
    }
    public class SchemaNameRender : IItemColumnRender
    {
        public IHtmlString Render(object dataItem, object value, ViewContext viewContext)
        {
            UrlHelper url = new UrlHelper(viewContext.RequestContext);

            return new HtmlString(string.Format(@"<a href=""{0}"" title=""{1}"">{1}</a>"
                , url.Action("Edit", viewContext.RequestContext.AllRouteValues().Merge("SchemaName", ((Schema)dataItem).Name))
                , value));
        }
    }
    [Grid(Checkable = true, IdProperty = "Name")]
    //[GridCommand(ActionName = "Delete", InheritRouteValues = true, Order = 2)]
    //[GridCommand(ActionName = "Export", InheritRouteValues = true, Order = 1)]
    [GridAction(ActionName = "Relations", RouteValueProperties = "schemaName=Name", Order = 1, Class = "o-icon relation dialog-link")]
    [GridAction(ActionName = "Edit", Class = "o-icon edit", RouteValueProperties = "schemaName=Name", Order = 2)]
    [GridAction(ActionName = "Copy", Class = "common-copy o-icon copy", Order = 0, RouteValueProperties = "sourceName=Name")]
    //[GridAction(ActionName = "Delete", Icon = "delete.png", ConfirmMessage = "Are you sure you want to delete this item?",
    //RouteValueProperties = "schemaName=Name", Order = 2)]
    public class Schema_Metadata
    {
        [Required(ErrorMessage = "Required")]
        [RemoteEx("IsNameAvailable", "Schema", RouteFields = "RepositoryName,SiteName", AdditionalFields = "old_Key")]
        [GridColumn(Order = 1, ItemRenderType = typeof(SchemaNameRender))]
        [RegularExpression(RegexPatterns.FileName, ErrorMessage = "A name cannot contain a space or any of the following characters:\\/:*?<>|~")]
        public string Name { get; set; }

        [DisplayName("Tree style data")]
        [Description("Create the tree style data and management interface.")]
        public bool IsTreeStyle
        {
            get;
            set;
        }
        //public bool Attachable { get; set; }

        //[GridColumn(Order = 2, HeaderText = "Child Content Type")]
        //[UIHint("ChildSchemas")]
        //[DataSource(typeof(ChildSchemaDataSource))]
        //[Display(Description = "Child ContentType")]
        //[DisplayName("Embedded content")]
        //[Description("Embed another content type. For example an <b>article</b> embeds <b>comments</b>")]
        //public IEnumerable<string> ChildSchemas { get; set; }
    }

    public class SchemaDataSource : ISelectListDataSource
    {

        private Repository Repository
        {
            get;
            set;
        }

        public SchemaDataSource()
        {
            this.Repository = Repository.Current;
        }

        public SchemaDataSource(string repository)
        {
            this.Repository = ServiceFactory.RepositoryManager.Get(repository);
        }

        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var repository = this.Repository;
            if (repository == null)
            {
                repository = Repository.Current;
            }
            return ServiceFactory.SchemaManager.All(repository, "")
                .Select(o => new SelectListItem
            {
                Text = o.Name,
                Value = o.Name
            });
        }
    }
}