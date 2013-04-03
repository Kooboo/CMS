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

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Extensions", Name = "Plugin", Order = 1)]
    public class AssemblyController : AdminControllerBase
    {
        public AssemblyController()
        {
            AssemblyManager = ServiceFactory.AssemblyManager;
        }
        public AssemblyManager AssemblyManager { get; set; }

        public virtual ActionResult Index()
        {
            return View(AssemblyManager.GetFiles(this.Site));
        }
        public virtual ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Upload(string file)
        {
            var entry = new JsonResultEntry();

            try
            {
                if (ModelState.IsValid)
                {
                    var files = this.HttpContext.Request.Files;

                    if (files.Count == 0 || files[0].ContentLength == 0 || !files[0].FileName.EndsWith(".dll", StringComparison.OrdinalIgnoreCase))
                    {
                        throw new KoobooException("Please upload a dll file.");
                    }

                    AssemblyManager.Upload(this.Site, files[0].FileName, files[0].InputStream);
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
        [HttpPost]
        public virtual ActionResult Delete(AssemblyFile[] model)
        {
            var entry = new JsonResultEntry();
            try
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
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.SetFailed().AddException(e);
            }
            return Json(entry);

            //return RedirectToAction("Index");
        }

        public virtual ActionResult ViewTypes(string fileName)
        {
            return View(AssemblyManager.GetTypes(Site, fileName).Select(it => new PluginType_Metadata(it)));

        }
    }
}
