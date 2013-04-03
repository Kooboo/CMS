using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Kooboo.Web;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Script.Serialization;
using Kooboo.Globalization;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites.Services;

using Kooboo.Web.Mvc;
using System.IO;
using Kooboo.CMS.Sites;


namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    public class CreateSiteAuthroziationAttribute : Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute
    {
        protected override bool AuthorizeCore(System.Web.Routing.RequestContext requestContext)
        {
            var allow = base.AuthorizeCore(requestContext);
            if (allow)
            {
                allow = ServiceFactory.UserManager.AllowCreatingSite(requestContext.HttpContext.User.Identity.Name);
            }
            return allow;
        }
    }
    public class SiteController : ControllerBase
    {
        //
        // GET: /Admin/Site/

        #region Create

        public enum SiteCreateType
        {
            CreateSubSite,
            CreateRootSite
        }
        [CreateSiteAuthroziation]
        public virtual ActionResult Create(SiteCreateType? createType)
        {
            if (createType == null)
            {
                if (string.IsNullOrWhiteSpace(ControllerContext.RequestContext.GetRequestValue("siteName")))
                {
                    if (!string.IsNullOrWhiteSpace(ControllerContext.RequestContext.GetRequestValue("parent")))
                    {
                        createType = SiteCreateType.CreateSubSite;
                    }
                    else
                    {
                        createType = SiteCreateType.CreateRootSite;
                    }
                }
                else
                {

                    createType = SiteCreateType.CreateSubSite;
                }
            }


            return RedirectToAction(createType.ToString(), ControllerContext.RequestContext.AllRouteValues());
        }

        [CreateSiteAuthroziation]
        [HttpPost]
        public virtual ActionResult Create([Bind(Exclude = "Parent")]Site site, string parent, string template)
        {

            JsonResultEntry resultEntry = new JsonResultEntry(ViewData.ModelState);

            try
            {
                if (ModelState.IsValid)
                {
                    Site parentSite = null;
                    if (!string.IsNullOrEmpty(parent))
                    {
                        parentSite = new Site(SiteHelper.SplitFullName(parent));
                    }
                    var createdSite = Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.Create(parentSite, site.Name, template, site, User.Identity.Name);

                    resultEntry.RedirectUrl = Url.Action("SiteMap", new { controller = "home", siteName = createdSite.FullName });
                }
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }

            return Json(resultEntry);
        }

        [CreateSiteAuthroziation]
        public virtual ActionResult CreateRootSite()
        {

            ViewData["TemplatesJSON"] = ServiceFactory.SiteTemplateManager.All().Select(o => { o.Thumbnail = !string.IsNullOrWhiteSpace(o.Thumbnail) ? Url.Content(o.Thumbnail) : null; return o; }).ToJSON();

            ViewData["TemplateCategorys"] = ServiceFactory.SiteTemplateManager.AllCategories();

            return View(new CreateSiteModel() { Version = "1.0.0.0" });
        }

        [CreateSiteAuthroziation]
        [HttpPost]
        public virtual ActionResult CreateRootSite([Bind(Exclude = "Parent")]Site site, string template, string parent)
        {
            return Create(site, parent, template);
        }

        [CreateSiteAuthroziation]
        public virtual ActionResult CreateSubSite()
        {
            string siteName = Request["siteName"] ?? Request["parent"];
            var createSiteModel = new CreateSiteModel() { Version = "1.0.0.0" };
            if (!string.IsNullOrWhiteSpace(siteName))
            {
                //createSiteModel.Parent = siteName;
                var parentSite = SiteHelper.Parse(siteName).AsActual();
                createSiteModel.Repository = parentSite.Repository;
                createSiteModel.Theme = parentSite.Theme;
                createSiteModel.Version = parentSite.Version;
            }

            return View(createSiteModel);
        }

        [CreateSiteAuthroziation]
        [HttpPost]
        public virtual ActionResult CreateSubSite([Bind(Exclude = "Parent")]Site site, string parent)
        {
            return Create(site, parent, null);
        }

        #endregion

        #region Validation
        [CreateSiteAuthroziation]
        public string CheckRepository(string name)
        {
            var repository = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Get(name);
            if (repository != null)
            {
                int i = 0;
                while (Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Get(name + i) != null)
                {
                    i++;
                }
                return (new { IsNew = false, ErrMsg = string.Format("{0} is not available. Try {1}.", name, name + i) }).ToJSON();
            }
            else
            {
                return (new { IsNew = true }).ToJSON();
            }
        }
        [CreateSiteAuthroziation]
        public JsonResult IsRepositoryAvaliable(string repository)
        {

            bool isNew = string.Compare(Request.RequestContext.GetRequestValue("IsNew"), "true", true) == 0;

            if (!isNew)
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            var entity = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Get(repository);

            if (entity != null)
            {
                int i = 0;
                while (Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Get(repository + i) != null)
                {
                    i++;
                }

                return Json(string.Format("{0} is not available. Try {1}.", repository, repository + i), JsonRequestBehavior.AllowGet);

            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [CreateSiteAuthroziation]
        public virtual ActionResult IsSiteNameAvailable(string name, string parent)
        {
            Site parentSite = null;
            if (!string.IsNullOrEmpty(parent))
            {
                parentSite = SiteHelper.Parse(parent);
            }
            var site = new Site(parentSite, name);
            if (!site.Exists())
            {
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            int i = 1;
            while (site.Exists())
            {
                site = new Site(parentSite, name + i.ToString());
                i++;
            }
            return Json(string.Format("{0} is not available. Try {1}.", name, site.Name), JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Delete
        public virtual ActionResult Delete(string siteName)
        {
            return View(new Site(siteName));
        }

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Settings", Name = "System", Order = 0)]
        [HttpPost]
        public virtual ActionResult Delete(string siteName, bool deleteRepository)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                if (!string.IsNullOrEmpty(siteName))
                {
                    var site = SiteHelper.Parse(siteName);
                    ServiceFactory.SiteManager.Remove(site, deleteRepository);
                    resultEntry.RedirectUrl = Url.Action("Index", "Home", ControllerContext.RequestContext.AllRouteValues().Merge("siteName", null));
                }
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);
        }
        public virtual ActionResult DeleteRepository(string siteName)
        {
            return View(new Site(siteName));
        }

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Settings", Name = "System", Order = 0)]
        [HttpPost]
        public virtual ActionResult DeleteRepository(string siteName, bool deleteRepository)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                if (!string.IsNullOrEmpty(siteName))
                {
                    var site = SiteHelper.Parse(siteName).AsActual();
                    if (site != null && !string.IsNullOrEmpty(site.Repository))
                    {
                        var sharedDb = ServiceFactory.SiteManager.UseSharedDB(site);
                        if (!sharedDb || (sharedDb && deleteRepository))
                        {
                            var repository = site.GetRepository();
                            Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Remove(repository);


                            site.Repository = null;
                            ServiceFactory.SiteManager.Update(site);
                        }
                    }

                }
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);
        }
        #endregion

        #region Export
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Settings", Name = "System", Order = 0)]
        public virtual ActionResult Export(string siteName)
        {
            if (!string.IsNullOrEmpty(siteName))
            {
                string fileName = siteName + ".zip";
                Response.AttachmentHeader(fileName);
                ServiceFactory.SiteManager.Export(siteName, Response.OutputStream);
            }

            return null;
        }

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Settings", Name = "System", Order = 0)]
        public virtual ActionResult ExportRepository(string siteName)
        {
            if (!string.IsNullOrEmpty(siteName))
            {
                var site = new Site(siteName).AsActual();
                if (site != null && !string.IsNullOrEmpty(site.Repository))
                {
                    string fileName = site.Repository + ".zip";
                    Response.AttachmentHeader(fileName);
                    Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Export(site.Repository, Response.OutputStream);
                }
            }

            return null;
        }
        #endregion

        #region Import
        [CreateSiteAuthroziation]
        public virtual ActionResult ImportSite()
        {
            return View();
        }
        [CreateSiteAuthroziation]
        [HttpPost]
        public virtual ActionResult ImportSite(ImportSiteModel model, bool uploadNew)
        {
            var entry = new JsonResultEntry();

            Site parent = null;
            if (!string.IsNullOrEmpty(model.Parent))
            {
                parent = new Site(model.Parent);
            }

            try
            {
                if (ModelState.IsValid)
                {

                    if (uploadNew)
                    {
                        if (!Request.Files[0].FileName.EndsWith(".zip"))
                        {
                            throw new KoobooException("Please upload an zip file".Localize());
                        }

                        ServiceFactory.SiteManager.Create(parent, model.Name, model.Repository, Request.Files[0].InputStream, User.Identity.Name);
                    }
                    else
                    {
                        ServiceFactory.SiteManager.Import(parent, model.Name, model.Repository, model.File, User.Identity.Name);
                    }
                }
                else
                {
                    entry.AddModelState(ModelState);
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }


        public ActionResult ImportRepository(string siteName)
        {
            return View();
        }

        [CreateSiteAuthroziation]
        [HttpPost]
        public virtual ActionResult ImportRepository(string siteName, ImportRepositoryModel model, bool uploadNew)
        {
            var entry = new JsonResultEntry();

            try
            {
                var site = new Site(siteName).AsActual();

                if (site != null && ModelState.IsValid)
                {
                    Kooboo.CMS.Content.Models.Repository repository = null;
                    if (uploadNew)
                    {
                        if (!Request.Files[0].FileName.EndsWith(".zip"))
                        {
                            throw new KoobooException("Please upload an zip file".Localize());
                        }

                        repository = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Create(model.Name, Request.Files[0].InputStream);
                    }
                    else
                    {
                        repository = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Create(model.Name, model.File);
                    }

                    if (repository != null)
                    {
                        site.Repository = repository.Name;
                        ServiceFactory.SiteManager.Update(site);
                    }
                }
                else
                {
                    entry.AddModelState(ModelState);
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }
        #endregion

        #region ONLINE OFFLINE

        public virtual ActionResult SwitchOffOn(string siteName)
        {
            var isOnLine = ServiceFactory.SiteManager.IsOnline(siteName);
            var entry = new JsonResultEntry();
            try
            {
                if (isOnLine)
                {
                    ServiceFactory.SiteManager.Offline(siteName);
                }
                else
                {
                    ServiceFactory.SiteManager.Online(siteName);
                }
                entry.AddMessage((isOnLine ? "Take offline " : "Take online").Localize()).SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

        #endregion

        #region Copy
        public ActionResult Copy(string siteName)
        {
            var site = new Site(siteName);
            CopySiteModel model = new CopySiteModel()
            {
                Parent = site.Parent == null ? "" : site.Parent.FullName
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Copy(string siteName,CopySiteModel model)
        {
            var site = new Site(siteName);

            var data = new JsonResultEntry(ViewData.ModelState);

            if (ModelState.IsValid)
            {
                try
                {
                    var createdSite = Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.Copy(site, model.Name, model.Repository);                    
                }
                catch (Exception e)
                {
                    Kooboo.HealthMonitoring.Log.LogException(e);
                }
            }

            return Json(data);

        }
        #endregion
    }
}