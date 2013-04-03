using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Services;
using System.IO;
using Kooboo.Web;

using Kooboo.Globalization;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Sites;
using Kooboo.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Extensions", Name = "Module", Order = 1)]
    public class ModuleController : AdminControllerBase
    {
        public ModuleController()
        {
            Manager = ServiceFactory.ModuleManager;
        }
        public ModuleManager Manager { get; set; }
        #region Management
        public virtual ActionResult Index()
        {
            var modules = Manager.All().Select(it => new ModuleListInSiteModel()
            {
                ModuleName = it,
                Included = Manager.SiteIsInModule(it, Site.FullName)
            });
            return View(modules);
        }
        [HttpPost]
        public virtual ActionResult Include(ModuleListInSiteModel[] model)
        {
            JsonResultEntry result = new JsonResultEntry(ModelState);

            if (ModelState.IsValid)
            {
                try
                {
                    foreach (var item in model)
                    {
                        Manager.AddSiteToModule(item.ModuleName, Site.FullName);
                    }
                    result.RedirectUrl = Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues());
                }
                catch (Exception e)
                {
                    result.AddException(e);
                }
            }

            return Json(result);
        }
        [HttpPost]
        public virtual ActionResult Exclude(string moduleName)
        {
            JsonResultEntry result = new JsonResultEntry(ModelState);

            if (ModelState.IsValid)
            {
                try
                {
                    Manager.RemoveSiteFromModule(moduleName, Site.FullName);

                    result.RedirectUrl = Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues());
                }
                catch (Exception e)
                {
                    result.AddException(e);
                }
            }

            return Json(result);
        }
        #endregion

        [ValidateInput(false)]
        public virtual ActionResult Scaffold(string moduleName, string moduleUrl)
        {
            ModuleAdminPosition position = new ModuleAdminPosition()
            {
                ModuleName = moduleName                
            };
            if (string.IsNullOrEmpty(moduleUrl))
            {
                moduleUrl = "~/admin";
            }
            var moduleActionResult = ModuleExecutor.InvokeAction(this.ControllerContext, Site, moduleUrl, position);
            if (ModuleActionResultExecutor.IsExclusiveResult(moduleActionResult.ActionResult))
            {
                return moduleActionResult.ActionResult;
            }
            else
            {
                var moduleHtml = ModuleExecutor.ExecuteActionResult(moduleActionResult);
                return View(moduleHtml);
            }
        }
    }
}
