using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web;
using Kooboo.Web.Script.Serialization;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites;
//using Kooboo.CMS.Web.ErrorHandling;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "Layout", Order = 1)]
    public class LayoutController : PathResourceControllerBase<Layout, LayoutManager>
    {
        #region CURD
        public virtual ActionResult Localize(string name)
        {
            Manager.Localize(name, Site);
            return RedirectToIndex();
        }
        public override ActionResult Create()
        {
            var engineName = ControllerContext.RequestContext.GetRequestValue("EngineName");
            ViewData["LayoutSamples"] = ServiceFactory.LayoutItemTemplateManager.GetLayoutSamples(engineName);
            //ViewData["CodeSnippets"] = ServiceFactory.LayoutCodeSnippetManager.All();
            ViewData["ViewEngine"] = Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(engineName);
            ViewBag.DefaultLayout = Kooboo.CMS.Sites.Services.ServiceFactory.LayoutItemTemplateManager.GetDefaultLayoutSample(engineName);
            return View();
        }
        //[CmsHandleErrorAttribute(Order = 1, RedirectToErrorPage = true)]
        public override ActionResult Edit(string name)
        {
            var layout = Manager.Get(Site, name);
            ViewData["LayoutSamples"] = ServiceFactory.LayoutItemTemplateManager.GetLayoutSamples(layout.EngineName);
            //ViewData["CodeSnippets"] = ServiceFactory.LayoutCodeSnippetManager.All();
            ViewData["ViewEngine"] = Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(layout.EngineName);
            ViewBag.DefaultLayout = Kooboo.CMS.Sites.Services.ServiceFactory.LayoutItemTemplateManager.GetDefaultLayoutSample(layout.EngineName);
            return View(layout);
        }
        [HttpPost]
        [ValidateInput(false)]
        public override ActionResult Create(Layout model)
        {
            JsonResultEntry resultEntry = new JsonResultEntry() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    model.UserName = User.Identity.Name;
                    Add(model);
                }
                resultEntry.SetSuccess();
                var pageRedirect = Request.Form["pageRedirect"];
                resultEntry.SetSuccess().RedirectUrl = pageRedirect.ToLower() == "false" ?
                    Url.Action("Edit", ControllerContext.RequestContext.AllRouteValues().Merge("Name", model.Name))
                    : Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues());
            }
            catch (Exception e)
            {
                resultEntry.SetFailed().AddException(e);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return Json(resultEntry);
            //return base.Create(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public override ActionResult Edit(Layout newModel, string old_key)
        {
            JsonResultEntry resultEntry = new JsonResultEntry() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    newModel.UserName = User.Identity.Name;

                    Update(newModel, old_key);
                    resultEntry.SetSuccess();
                    //resultEntry.RedirectUrl = this.Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues());

                    var pageRedirect = Request.Form["pageRedirect"];
                    resultEntry.AddMessage("The item has been saved.".Localize()).SetSuccess().RedirectUrl = string.Equals(pageRedirect, "true", StringComparison.OrdinalIgnoreCase) ? this.Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues()) : null;
                }
            }
            catch (Exception e)
            {
                resultEntry.SetFailed().AddException(e);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return Json(resultEntry);
        }
        public virtual ActionResult Relations(string name)
        {
            var model = Manager.RelationsPages(new Layout() { Site = Site, Name = name }).Select(o => new RelationModel
            {
                RelationName = o.FriendlyName,
                RelationType = "Page".Localize()
            });
            return View("Relations", model);
        }


        /// <summary>
        /// for remote validation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="old_Key"></param>
        /// <returns></returns>
        public virtual ActionResult IsNameAvailable(string name, string old_Key)
        {
            if (old_Key == null || !name.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (Manager.Get(Site, name) != null)
                {
                    return Json("The name already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Copy

        public virtual ActionResult Copy(string sourceName, string destName)
        {
            var entry = new JsonResultEntry();

            try
            {
                Manager.Copy(Site, sourceName, destName);
                entry.RedirectUrl = this.GetReturnUrl();
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);

        }
        public virtual ActionResult CopyNameAvailabled(string sourceName, string destName)
        {
            return this.IsNameAvailable(destName, null);
        }

        #endregion

        #region Versioning
        public virtual ActionResult Version(string name)
        {
            var layout = new Layout(Site, name);
            var versions = Kooboo.CMS.Sites.Versioning.VersionManager.AllVersions(layout);

            return View(versions);
        }
        public virtual ActionResult PreviewVersion(string name, int version)
        {
            var layout = new Layout(Site, name);

            var versionedLayout = Kooboo.CMS.Sites.Versioning.VersionManager.GetVersion(layout, version);

            return View(versionedLayout);
        }
        public virtual ActionResult Revert(string name, int version)
        {
            JsonResultEntry result = new JsonResultEntry();

            try
            {
                var layout = new Layout(Site, name);
                Kooboo.CMS.Sites.Versioning.VersionManager.Revert(layout, version);
                result.AddMessage("Revert successfully.".Localize());
            }
            catch (Exception e)
            {
                result.AddException(e);
            }

            return Json(result, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Import/Export
        #region Import
        public virtual ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Import(bool @override)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    Manager.Provider.Import(Site, Request.Files[0].InputStream, @override);
                }
                resultEntry.ReloadPage = true;
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry, "text/plain", System.Text.Encoding.UTF8);
        }
        #endregion

        #region Export
        [HttpPost]
        public virtual void Export(Kooboo.CMS.Sites.Models.Layout[] model)
        {
            var fileName = "Layouts.zip";
            Response.AttachmentHeader(fileName);
            foreach (var item in model)
            {
                item.Site = Site;
            }
            Manager.Provider.Export(model, Response.OutputStream);
        }
        #endregion
        #endregion

    }
}
