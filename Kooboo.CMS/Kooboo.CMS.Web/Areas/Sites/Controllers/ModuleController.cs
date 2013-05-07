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
using Kooboo.CMS.Common;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Extensions", Name = "Module", Order = 1)]
    public class ModuleController : Kooboo.CMS.Sites.AreaControllerBase
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
        public virtual ActionResult Include(ModuleListInSiteModel[] model, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {
                        Manager.AddSiteToModule(item.ModuleName, Site.FullName);
                    }
                    resultData.RedirectUrl = @return;
                }
            });

            return Json(data);
        }
        [HttpPost]
        public virtual ActionResult Exclude(ModuleListInSiteModel[] model, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    foreach (var item in model)
                    {
                        Manager.RemoveSiteFromModule(item.ModuleName, Site.FullName);
                    }

                    resultData.RedirectUrl = @return;
                }
            });

            return Json(data);
        }
        #endregion
    }
}
