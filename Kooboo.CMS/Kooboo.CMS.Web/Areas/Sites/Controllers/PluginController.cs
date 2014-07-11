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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.CMS.Sites;
using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Extensions", Name = "Plugin", Order = 1)]
    public class PluginController : Kooboo.CMS.Sites.AreaControllerBase
    {
        #region .ctor
        public PluginController()
        {
            AssemblyManager = ServiceFactory.AssemblyManager;
        }
        public AssemblyManager AssemblyManager { get; set; }
        #endregion

        #region Index
        public virtual ActionResult Index()
        {
            return View(AssemblyManager.GetFiles(this.Site));
        }
        #endregion

        #region Upload
        public virtual ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Upload(string file, string @return)
        {
            var files = this.HttpContext.Request.Files;

            if (files.Count == 0 || files[0].ContentLength == 0 || !files[0].FileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
            {
            }
            else
            {
                AssemblyManager.Upload(this.Site, files[0].FileName, files[0].InputStream);
            }


            return Redirect(@return);
        }
        #endregion

        #region Delete
        [HttpPost]
        public virtual ActionResult Delete(AssemblyFile[] model, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                if (model != null)
                {
                    foreach (var fileName in model)
                    {
                        if (!string.IsNullOrEmpty(fileName.FileName))
                        {
                            AssemblyManager.Delete(Site, fileName.FileName);
                        }
                    }
                }
                resultData.RedirectUrl = @return;
            });
            return Json(data);
        }
        #endregion

        #region ViewTypes
        public virtual ActionResult ViewTypes(string fileName)
        {
            return View(AssemblyManager.GetTypes(Site, fileName).Select(it => new PluginType_Metadata(it)));

        }
        #endregion
    }
}
