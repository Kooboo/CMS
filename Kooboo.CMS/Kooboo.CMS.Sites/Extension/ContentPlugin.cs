using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.View;
using System.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.CMS.Content.Models.Binder;
namespace Kooboo.CMS.Sites.Extension
{
    public abstract class ContentPlugin : IPagePlugin
    {
        protected abstract void ByFolder(Page_Context pageContext, View.PagePositionContext positionContext, Repository repository, TextFolder folder);
        protected abstract void BySchema(Page_Context pageContext, View.PagePositionContext positionContext, Repository repository, Schema schema);
        public virtual System.Web.Mvc.ActionResult Execute(View.Page_Context pageContext, View.PagePositionContext positionContext)
        {
            var httpContext = pageContext.ControllerContext.RequestContext.HttpContext;

            if (httpContext.Request.HttpMethod.ToUpper() == "POST")
            {
                var site = pageContext.PageRequestContext.Site;

                var repository = site.GetRepository();
                if (repository == null)
                {
                    throw new SiteRepositoryNotExists();
                }
                var folderName = pageContext.ControllerContext.RequestContext.GetRequestValue("FolderName");
                if (!string.IsNullOrEmpty(folderName))
                {
                    var folder = FolderHelper.Parse<TextFolder>(repository, folderName);
                    ByFolder(pageContext, positionContext, repository, folder);
                }
                else
                {
                    var schemaName = pageContext.ControllerContext.RequestContext.GetRequestValue("SchemaName");
                    if (!string.IsNullOrEmpty(schemaName))
                    {
                        var schema = new Schema(repository, schemaName);

                        BySchema(pageContext, positionContext, repository, schema);
                    }
                }
            }

            return null;
        }

        public static IEnumerable<TextContent> GetCategories(string modelName, ControllerContext controllerContext)
        {
            var modelState = new ModelStateDictionary();
            ModelBindingContext bindingContext = new ModelBindingContext()
            {
                FallbackToEmptyPrefix = true,
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(CategoryContent[])),
                ModelName = modelName,
                ModelState = modelState,
                ValueProvider = controllerContext.Controller.ValueProvider
            };
            DefaultModelBinder modelBinder = new DefaultModelBinder();
            var model = modelBinder.BindModel(controllerContext, bindingContext);
            if (model == null)
            {
                return null;
            }
            return ((IEnumerable<CategoryContent>)model).Select(it => new TextContent() { FolderName = it.FolderName, UUID = it.UUID });
        }

        public abstract string Description { get; }
    }
    public class CategoryContent
    {
        public string FolderName { get; set; }
        public string UUID { get; set; }
    }
    /// <summary>
    /// 
    /// </summary>
    public class AddTextContentPlugin : ContentPlugin
    {
        /// <summary>
        /// <example>
        /// <div>
        //        <div>
        //    <form method="post">
        //    <input type="hidden" name="FolderName" value="news" />
        //    <input type="hidden" name="Published" value="true" />
        //    <input type="hidden" name="Categories[0].FolderName" value="<%: ViewBag.category.FolderName %>" />
        //    <input type="hidden" name="Categories[0].UUID" value="<%: ViewBag.category.UUID %>" />
        //    <table>
        //        <tr>
        //            <td>
        //                title:
        //            </td>
        //            <td>
        //                <input type="text" name="title" data-val-required="title is required" data-val="true" />
        //                <%: Html.ValidationMessageForInput("title") %>
        //            </td>
        //        </tr>
        //        <tr>
        //            <td>
        //                body:
        //            </td>
        //            <td>
        //                <textarea name="body" cols="20" rows="10"></textarea>
        //            </td>
        //        </tr>
        //    </table>
        //    <input type="submit" name="submit" value="submit" />
        //    </form>
        //</div>
        /// </example>
        /// </summary>
        /// <param name="pageContext"></param>
        /// <param name="repository"></param>
        /// <param name="folder"></param>
        protected override void ByFolder(Page_Context pageContext, View.PagePositionContext positionContext, Repository repository, TextFolder folder)
        {
            var httpContext = pageContext.ControllerContext.RequestContext.HttpContext;

            var categories = GetCategories("Categories", pageContext.ControllerContext);

            try
            {
                var parentFolder = httpContext.Request.Form["ParentFolder"];
                var parentUUID = httpContext.Request.Form["ParentUUID"];
                Content.Services.ServiceFactory.TextContentManager.Add(repository, folder, parentFolder, parentUUID,
                  httpContext.Request.Form, httpContext.Request.Files, categories, httpContext.User.Identity.Name);
            }
            catch (RuleViolationException violationException)
            {
                violationException.FillIssues(positionContext == null ? pageContext.ControllerContext.Controller.ViewData.ModelState : positionContext.ViewData.ModelState);
            }
        }



        protected override void BySchema(Page_Context pageContext, View.PagePositionContext positionContext, Repository repository, Schema schema)
        {
            var httpContext = pageContext.ControllerContext.RequestContext.HttpContext;

            string parentUUID = pageContext.ControllerContext.RequestContext.GetRequestValue("ParentUUID");
            try
            {
                Content.Services.ServiceFactory.TextContentManager.Add(repository, schema, parentUUID,
                       httpContext.Request.Form, httpContext.Request.Files, httpContext.User.Identity.Name);
            }
            catch (RuleViolationException violationException)
            {
                violationException.FillIssues(positionContext == null ? pageContext.ControllerContext.Controller.ViewData.ModelState : positionContext.ViewData.ModelState);
            }

        }

        public override string Description
        {
            get
            {
                return string.Format("The page plugin for adding text content, sample html code:\r\n{0}".Localize(),
                    @"
    <form method=""post"">
    <input type=""hidden"" name=""FolderName"" value=""news"" />
    <input type=""hidden"" name=""Published"" value=""true"" />
    <input type=""hidden"" name=""Categories[0].FolderName"" value=""<%: ViewBag.category.FolderName %>"" />
    <input type=""hidden"" name=""Categories[0].UUID"" value=""<%: ViewBag.category.UUID %>"" />
    <table>
        <tr>
            <td>
                title:
            </td>
            <td>
                <input type=""text"" name=""title"" data-val-required=""title is required"" data-val=""true"" />
                <%: Html.ValidationMessageForInput(""title"") %>
            </td>
        </tr>
        <tr>
            <td>
                body:
            </td>
            <td>
                <textarea name=""body"" cols=""20"" rows=""10""></textarea>
            </td>
        </tr>
    </table>
    <input type=""submit"" name=""submit"" value=""submit"" />
    </form>");
            }
        }
    }

    public class UpdateTextContentPlugin : ContentPlugin
    {
        protected override void ByFolder(Page_Context pageContext, View.PagePositionContext positionContext, Repository repository, TextFolder folder)
        {
            var httpContext = pageContext.ControllerContext.RequestContext.HttpContext;
            string uuid = pageContext.ControllerContext.RequestContext.GetRequestValue("uuid");

            var addCategories = GetCategories("AddCategories", pageContext.ControllerContext);
            var removeCategories = GetCategories("RemoveCategories", pageContext.ControllerContext);
            try
            {
                Content.Services.ServiceFactory.TextContentManager.Update(repository, folder, uuid, httpContext.Request.Form
                    , httpContext.Request.Files, DateTime.UtcNow, addCategories, removeCategories, httpContext.User.Identity.Name);
            }
            catch (RuleViolationException violationException)
            {
                violationException.FillIssues(positionContext == null ? pageContext.ControllerContext.Controller.ViewData.ModelState : positionContext.ViewData.ModelState);
            }
        }

        protected override void BySchema(Page_Context pageContext, View.PagePositionContext positionContext, Repository repository, Schema schema)
        {
            var httpContext = pageContext.ControllerContext.RequestContext.HttpContext;
            string uuid = pageContext.ControllerContext.RequestContext.GetRequestValue("uuid");

            try
            {
                Content.Services.ServiceFactory.TextContentManager.Update(repository, schema, uuid, httpContext.Request.Form
                    , httpContext.Request.Files, httpContext.User.Identity.Name);
            }
            catch (RuleViolationException violationException)
            {
                violationException.FillIssues(positionContext == null ? pageContext.ControllerContext.Controller.ViewData.ModelState : positionContext.ViewData.ModelState);
            }
        }

        public override string Description
        {
            get { return "The page plugin for updating text content.".Localize(); }
        }
    }

    public class DeleteTextContentPlugin : ContentPlugin
    {

        protected override void ByFolder(Page_Context pageContext, View.PagePositionContext positionContext, Repository repository, TextFolder folder)
        {
            string uuid = pageContext.ControllerContext.RequestContext.GetRequestValue("uuid");
            try
            {
                Content.Services.ServiceFactory.TextContentManager.Delete(repository, folder, uuid);
            }
            catch (RuleViolationException violationException)
            {
                violationException.FillIssues(positionContext == null ? pageContext.ControllerContext.Controller.ViewData.ModelState : positionContext.ViewData.ModelState);
            }
        }

        protected override void BySchema(Page_Context pageContext, View.PagePositionContext positionContext, Repository repository, Schema schema)
        {
            string uuid = pageContext.ControllerContext.RequestContext.GetRequestValue("uuid");
            try
            {
                Content.Services.ServiceFactory.TextContentManager.Delete(repository, schema, uuid);
            }
            catch (RuleViolationException violationException)
            {
                violationException.FillIssues(positionContext == null ? pageContext.ControllerContext.Controller.ViewData.ModelState : positionContext.ViewData.ModelState);
            }
        }

        public override string Description
        {
            get
            {
                return string.Format("The page plugin for deleting text content. Sample htmle code:\r\n{0}".Localize(),
                    @"
                <form action=""<%:Url.FrontUrl().PageUrl(""news/delete"")%>"" method=""post"">
                <input type=""hidden"" name=""uuid"" value=""<%: ViewBag.newsdetail.uuid %>"" />
                <input type=""hidden"" name=""FolderName"" value=""<%: ViewBag.newsdetail.folderName %>"" />
                <input type=""submit"" name=""deleteBtn"" value=""delete"" />
                </form>");
            }
        }
    }
}
