#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Caching;
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Caching;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Caching;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;
using Kooboo.Web;
using Kooboo.Web.Mvc;
using Kooboo.Web.Script.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Web;
using System.Web.Mvc;
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
    public class SiteController : Kooboo.CMS.Sites.AreaControllerBase
    {
        #region Create
        [CreateSiteAuthroziation]
        public virtual ActionResult Create()
        {
            return View();
        }
        [CreateSiteAuthroziation]
        [HttpPost]
        public virtual ActionResult Create([Bind(Exclude = "Parent")]Site site, string template)
        {
            return CreateSite(site, null, template);
        }

        protected virtual ActionResult CreateSite([Bind(Exclude = "Parent")]Site site, string parent, string template)
        {
            var data = new JsonResultData(ViewData.ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    Site parentSite = null;
                    if (!string.IsNullOrEmpty(parent))
                    {
                        parentSite = new Site(SiteHelper.SplitFullName(parent));
                    }
                    var createdSite = Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.Create(parentSite, site.Name, template, site, User.Identity.Name);

                    resultData.RedirectUrl = Url.Action("SiteMap", new { controller = "Home", siteName = createdSite.FullName });
                });
            }

            return Json(data);
        }

        #region CreateSubSite
        [CreateSiteAuthroziation]
        public virtual ActionResult CreateSubSite(CreateSiteModel model)
        {
            string siteName = Request["siteName"] ?? Request["parent"];
            if (!string.IsNullOrWhiteSpace(siteName))
            {
                var parentSite = SiteHelper.Parse(siteName).AsActual();
                model.Repository = parentSite.Repository;
            }
            ModelState.Clear();
            return View(model);
        }

        [CreateSiteAuthroziation]
        [HttpPost]
        public virtual ActionResult CreateSubSite([Bind(Exclude = "Parent")]Site site, string parent)
        {
            return CreateSite(site, parent, null);
        }
        #endregion

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
        public JsonResult IsRepositoryAvaliable(string repository, bool? createNew)
        {
            if (createNew.HasValue && createNew.Value == false)
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

                return Json(string.Format("{0} is not available. Try {1}.".Localize(), repository, repository + i), JsonRequestBehavior.AllowGet);

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

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Name = "Settings", Group = "System", Order = 0)]
        [HttpPost]
        public virtual ActionResult Delete(string siteName, bool deleteRepository)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (!string.IsNullOrEmpty(siteName))
                {
                    var site = SiteHelper.Parse(siteName);
                    ServiceFactory.SiteManager.Remove(site, deleteRepository);
                    resultData.RedirectUrl = Url.Action("Index", "Home", ControllerContext.RequestContext.AllRouteValues().Merge("siteName", null));
                }
            });
            return Json(data);
        }
        public virtual ActionResult DeleteRepository(string siteName)
        {
            return View(new Site(siteName));
        }

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Name = "Settings", Group = "System", Order = 0)]
        [HttpPost]
        public virtual ActionResult DeleteRepository(string siteName, bool deleteRepository)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
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
            });
            return Json(data);
        }
        #endregion

        #region Export
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Name = "Settings", Group = "System", Order = 0)]
        public virtual ActionResult Export(string siteName)
        {
            return View(new ExportSiteModel() { SiteName = siteName });
        }
        [HttpPost]
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Name = "Settings", Group = "System", Order = 0)]
        public virtual ActionResult Export(ExportSiteModel model, string @return)
        {
            string fileName = model.SiteName + ".zip";

            switch (model.ExportType)
            {
                case ExportType.SiteWithDatabase:
                    Response.AttachmentHeader(fileName);
                    ServiceFactory.SiteManager.Export(model.SiteName, Response.OutputStream, true, model.IncludeSubSites);
                    break;
                case ExportType.OnlySite:
                    Response.AttachmentHeader(fileName);
                    ServiceFactory.SiteManager.Export(model.SiteName, Response.OutputStream, false, model.IncludeSubSites);
                    break;
                case ExportType.OnlitDatabase:
                    var site = new Site(model.SiteName).AsActual();
                    if (site != null && !string.IsNullOrEmpty(site.Repository))
                    {
                        fileName = site.Repository + ".zip";
                        Response.AttachmentHeader(fileName);
                        Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Export(site.Repository, Response.OutputStream);
                    }
                    break;
                default:
                    break;
            }
            return null;
        }
        //[Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Name = "Settings", Group = "System", Order = 0)]
        //public virtual ActionResult ExportSite(string siteName)
        //{
        //    if (!string.IsNullOrEmpty(siteName))
        //    {
        //        string fileName = siteName + ".zip";
        //        Response.AttachmentHeader(fileName);
        //        ServiceFactory.SiteManager.Export(siteName, Response.OutputStream, false);
        //    }

        //    return null;
        //}
        //[Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Name = "Settings", Group = "System", Order = 0)]
        //public virtual ActionResult ExportRepository(string siteName)
        //{
        //    if (!string.IsNullOrEmpty(siteName))
        //    {
        //        var site = new Site(siteName).AsActual();
        //        if (site != null && !string.IsNullOrEmpty(site.Repository))
        //        {
        //            string fileName = site.Repository + ".zip";
        //            Response.AttachmentHeader(fileName);
        //            Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Export(site.Repository, Response.OutputStream);
        //        }
        //    }

        //    return null;
        //}
        #endregion

        #region Import
        [CreateSiteAuthroziation]
        public virtual ActionResult ImportSite()
        {
            return View();
        }
        [CreateSiteAuthroziation]
        [HttpPost]
        public virtual ActionResult ImportSite(ImportSiteModel model, string @return)
        {
            var data = new JsonResultData(ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    Site parent = null;
                    if (!string.IsNullOrEmpty(model.Parent))
                    {
                        parent = new Site(model.Parent);
                    }
                    Site createdSite = null;
                    if (Request.Files.Count > 0)
                    {
                        createdSite = ServiceFactory.SiteManager.Create(parent, model.Name, Request.Files[0].InputStream, model.ToSiteSetting(), User.Identity.Name);
                    }
                    else
                    {
                        createdSite = ServiceFactory.SiteManager.Import(parent, model.Name, model.File, model.ToSiteSetting(), User.Identity.Name);
                    }

                    resultData.RedirectUrl = Url.Action("SiteMap", new { controller = "Home", siteName = createdSite.FullName });
                });
            }
            return Json(data);
        }


        public ActionResult ImportRepository(string siteName)
        {
            return View();
        }

        [CreateSiteAuthroziation]
        [HttpPost]
        public virtual ActionResult ImportRepository(string siteName, ImportRepositoryModel model, string @return)
        {
            var data = new JsonResultData(ModelState);

            var site = new Site(siteName).AsActual();
            if (site != null && ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                    {
                        Kooboo.CMS.Content.Models.Repository repository = null;
                        if (Request.Files.Count > 0)
                        {
                            repository = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Create(model.Repository, Request.Files[0].InputStream);
                        }
                        else
                        {
                            repository = Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Create(model.Repository, model.File);
                        }

                        var oldRepository = site.Repository;
                        if (repository != null)
                        {
                            site.Repository = repository.Name;
                            ServiceFactory.SiteManager.Update(site);
                        }
                        try
                        {
                            if (model.DeleteTheOldRepository)
                            {
                                repository = new Kooboo.CMS.Content.Models.Repository(oldRepository);
                                if (!Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.IfTheRepositoryUsedByOtherSites(site, oldRepository))
                                {
                                    Kooboo.CMS.Content.Services.ServiceFactory.RepositoryManager.Remove(repository);
                                };
                            }
                        }
                        catch (Exception e)
                        {
                            Kooboo.HealthMonitoring.Log.LogException(e);
                        }

                        resultData.RedirectUrl = @return;
                    });
            }

            return Json(data);
        }
        #endregion

        #region ONLINE OFFLINE

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Settings", Order = 1)]
        public virtual ActionResult SwitchOffOn(string siteName)
        {
            var isOnLine = ServiceFactory.SiteManager.IsOnline(siteName);
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
                {
                    if (isOnLine)
                    {
                        ServiceFactory.SiteManager.Offline(siteName);
                    }
                    else
                    {
                        ServiceFactory.SiteManager.Online(siteName);
                    }
                    resultData.ReloadPage = true;
                });
            return Json(data);
        }

        #endregion

        #region ClearCache
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Settings", Order = 1)]
        [HttpPost]
        public virtual ActionResult ClearCache()
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                Site.ClearCache();
                var repository = Site.GetRepository();
                if (repository != null)
                {
                    repository.ClearCache();
                }
                CacheManagerFactory.DefaultCacheManager.ClearGlobalObjectCache();

                data.AddMessage("The site cache has been cleared.".Localize());
            });

            return Json(data);
        }
        #endregion

        #region Settings
        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Settings", Order = 1)]
        public virtual ActionResult Settings()
        {
            return View(Site.AsActual());
        }

        [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Settings", Order = 1)]
        [HttpPost]
        public virtual ActionResult Settings(Site model)
        {
            JsonResultData data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    var site = Site.AsActual();

                    //if (isNew)
                    //{
                    //    CreateRepository(model.Repository);
                    //}

                    site.DisplayName = model.DisplayName;
                    site.Culture = model.Culture;

                    site.Domains = model.Domains;
                    site.SitePath = model.SitePath;
                    site.Mode = model.Mode;
                    site.Repository = model.Repository;
                    site.Theme = model.Theme;
                    site.Version = model.Version;
                    site.InlineEditing = model.InlineEditing;
                    site.ShowSitemap = model.ShowSitemap;
                    site.Smtp = model.Smtp;
                    site.Security = model.Security;
                    site.ResourceDomain = model.ResourceDomain;

                    if (site.Smtp != null && site.Smtp.To != null)
                    {
                        site.Smtp.To = site.Smtp.To.Where(it => !string.IsNullOrEmpty(it)).ToArray();
                    }
                    site.CustomFields = model.CustomFields;
                    site.EnableVersioning = model.EnableVersioning;
                    site.EnableJquery = model.EnableJquery;
                    site.EnableStyleEdting = model.EnableStyleEdting;
                    site.TimeZoneId = model.TimeZoneId;
                    site.HtmlMeta = model.HtmlMeta;
                    site.Membership = model.Membership;

                    ServiceFactory.SiteManager.Update(site);
                    resultData.AddMessage("Site setting has been changed.".Localize());
                }

            });
            return Json(data);
        }

        #endregion

        #region TestSMTP
        public ActionResult TestSMTP(Site site)
        {
            JsonResultData data = new JsonResultData();
            data.RunWithTry((resultData) =>
            {
                if (string.IsNullOrEmpty(site.Smtp.Host))
                {
                    resultData.AddErrorMessage("The SMTP server is required.".Localize());
                }
                if (string.IsNullOrEmpty(site.Smtp.From))
                {
                    resultData.AddErrorMessage("The FROM address is required.".Localize());
                }
                if (site.Smtp.To == null || site.Smtp.To.Length == 0 || string.IsNullOrEmpty(site.Smtp.To.First()))
                {
                    resultData.AddErrorMessage("The TO addresses is required.".Localize());
                }
                if (data.Success)
                {
                    MailMessage message = new MailMessage() { From = new MailAddress(site.Smtp.From) };
                    foreach (var item in site.Smtp.To)
                    {
                        if (!string.IsNullOrEmpty(item))
                        {
                            message.To.Add(item);
                        }
                    }
                    message.Subject = "The test email from Kooboo CMS".Localize();
                    message.Body = "It is a test email sent from Kooboo CMS to verify the SMTP setting.".Localize();
                    SmtpClient smtpClient = site.Smtp.ToSmtpClient();

                    smtpClient.Send(message);

                    resultData.AddMessage("The test email sent successfully.".Localize());
                }

            });
            return Json(data);
        }
        #endregion

        #region Copy
        public ActionResult Copy()
        {
            CopySiteModel model = new CopySiteModel()
            {
                Parent = Site.Parent == null ? "" : Site.Parent.FullName
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult Copy(CopySiteModel model)
        {
            var data = new JsonResultData(ViewData.ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    if (string.IsNullOrEmpty(model.Membership))
                    {
                        model.Membership = Site.AsActual().Membership;
                    }
                    var createdSite = Kooboo.CMS.Sites.Services.ServiceFactory.SiteManager.Copy(Site, model.Name, model.ToSiteSetting());

                    resultData.RedirectUrl = Url.Action("SiteMap", new { controller = "Home", siteName = createdSite.FullName });
                });
            }

            return Json(data);

        }
        #endregion
    }
}