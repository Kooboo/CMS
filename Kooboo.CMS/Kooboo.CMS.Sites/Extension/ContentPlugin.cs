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
using System.Text;
using System.Web.Helpers;
using System.Web.Routing;
using System.Web.Mvc;
using System.Web;
using System.Collections.Specialized;
using System.Globalization;

using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.Common.Globalization;
using Kooboo.CMS.Content.Models.Binder;
using Kooboo.Common.ObjectContainer;
using Kooboo.Common.TokenTemplate;

namespace Kooboo.CMS.Sites.Extension
{
    #region ContentPlugin
    public abstract class ContentPlugin : IPagePlugin, IHttpMethodPagePlugin, ISubmissionPlugin
    {
        #region DoPost
        protected abstract object DoPost(Repository repository, TextFolder folder, ControllerContext controllerContext, NameValueCollection formValues);
        #endregion

        #region Execute
        public virtual System.Web.Mvc.ActionResult Execute(View.Page_Context pageContext, View.PagePositionContext positionContext)
        {
            return null;
        }

        #endregion

        #region GetCategories
        public static IEnumerable<TextContent> GetCategories(string modelName, ControllerContext controllerContext, NameValueCollection formValues)
        {
            var modelState = new ModelStateDictionary();
            ModelBindingContext bindingContext = new ModelBindingContext()
            {
                FallbackToEmptyPrefix = true,
                ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(null, typeof(CategoryContent[])),
                ModelName = modelName,
                ModelState = modelState,
                ValueProvider = new ValueProviderCollection(new List<System.Web.Mvc.IValueProvider>() { new NameValueCollectionValueProvider(formValues, formValues, CultureInfo.CurrentCulture), controllerContext.Controller.ValueProvider })
            };
            DefaultModelBinder modelBinder = new DefaultModelBinder();
            var model = modelBinder.BindModel(controllerContext, bindingContext);
            if (model == null)
            {
                return null;
            }
            return ((IEnumerable<CategoryContent>)model).Select(it => new TextContent() { FolderName = it.FolderName, UUID = it.UUID });
        }
        #endregion

        #region HttpGet
        public ActionResult HttpGet(Page_Context context, PagePositionContext positionContext)
        {
            return null;
        }
        #endregion

        #region HttpPost
        public ActionResult HttpPost(Page_Context context, PagePositionContext positionContext)
        {
            var httpContext = context.ControllerContext.RequestContext.HttpContext;

            var site = context.PageRequestContext.Site;

            var repository = site.GetRepository();
            if (repository == null)
            {
                throw new SiteRepositoryNotExists();
            }
            object model = null;
            Exception exception = null;
            try
            {
                var folderName = context.ControllerContext.RequestContext.GetRequestValue("FolderName");
                if (!string.IsNullOrEmpty(folderName))
                {
                    var folder = FolderHelper.Parse<TextFolder>(repository, folderName);
                    model = DoPost(repository, folder, context.ControllerContext, context.ControllerContext.HttpContext.Request.Form);
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            return PluginHelper.ReturnActionResult(context.ControllerContext, model, exception);
        }
        #endregion

        #region Submit

        public ActionResult Submit(Site site, ControllerContext controllerContext, SubmissionSetting submissionSetting)
        {
            object model = null;
            Exception exception = null;
            var formValues = new NameValueCollection(controllerContext.HttpContext.Request.Unvalidated().Form);
            try
            {
                var repository = site.GetRepository();
                var valueProvider = new MvcValueProvider(controllerContext.Controller.ValueProvider);
                formValues = PluginHelper.ApplySubmissionSettings(submissionSetting, formValues, valueProvider);
                var folderName = formValues["FolderName"];
                if (!string.IsNullOrEmpty(folderName))
                {
                    var folder = FolderHelper.Parse<TextFolder>(repository, folderName);
                    model = DoPost(repository, folder, controllerContext, formValues);
                }
            }
            catch (Exception e)
            {
                exception = e;
            }

            return PluginHelper.ReturnActionResult(controllerContext, model, exception);
        }
        #endregion



        #region Parameters
        public Dictionary<string, object> Parameters
        {
            get
            {
                return new Dictionary<string, object>() { 
                    {"FolderName","Articles"},
                    {"Published","true"},
                    {"ParentFolder",""}
                };
            }
        }
        #endregion
    }
    #endregion

    #region CategoryContent
    public class CategoryContent
    {
        public string FolderName { get; set; }
        public string UUID { get; set; }
    }
    #endregion

    #region AddTextContentPlugin
    public class AddTextContentPlugin : ContentPlugin
    {
        #region DoPost
        protected override object DoPost(Repository repository, TextFolder folder, ControllerContext controllerContext, NameValueCollection formValues)
        {
            #region Example
            // <example>
            // <div>
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
            // </example> 
            #endregion

            var httpContext = controllerContext.HttpContext;
            var categories = GetCategories("Categories", controllerContext, formValues);

            var parentFolder = formValues["ParentFolder"];
            var parentUUID = formValues["ParentUUID"];
            return Content.Services.ServiceFactory.TextContentManager.Add(repository, folder, parentFolder, parentUUID,
               formValues, httpContext.Request.Files, categories, httpContext.User.Identity.Name);
        }
        #endregion
    }
    #endregion

    #region UpdateTextContentPlugin
    public class UpdateTextContentPlugin : ContentPlugin
    {
        #region DoPost
        protected override object DoPost(Repository repository, TextFolder folder, ControllerContext controllerContext, NameValueCollection formValues)
        {
            var httpContext = controllerContext.HttpContext;
            string uuid = controllerContext.RequestContext.GetRequestValue("uuid");

            var addCategories = GetCategories("AddCategories", controllerContext, formValues);
            var removeCategories = GetCategories("RemoveCategories", controllerContext, formValues);
            return Content.Services.ServiceFactory.TextContentManager.Update(repository, folder, uuid, formValues
                , httpContext.Request.Files, DateTime.UtcNow, addCategories, removeCategories, httpContext.User.Identity.Name);
        }
        #endregion
    }
    #endregion

    #region DeleteTextContentPlugin
    public class DeleteTextContentPlugin : ContentPlugin
    {
        public string Description
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

        protected override object DoPost(Repository repository, TextFolder folder, ControllerContext controllerContext, NameValueCollection formValues)
        {
            string uuid = controllerContext.RequestContext.GetRequestValue("uuid");

            Content.Services.ServiceFactory.TextContentManager.Delete(repository, folder, uuid);

            return null;
        }
    }
    #endregion
}
