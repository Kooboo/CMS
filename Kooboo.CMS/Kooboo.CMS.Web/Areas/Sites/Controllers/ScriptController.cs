using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites;
using Kooboo.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "Script", Order = 1)]
    public class ScriptController : PathResourceControllerBase<ScriptFile, ScriptManager>
    {
        public override ActionResult Edit(string fileName)
        {
            return base.Edit(fileName);
        }
        /// <summary>
        /// for remote validation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="old_Key"></param>
        /// <returns></returns>
        public virtual ActionResult IsNameAvailable(string name, string fileExtension, string old_Key)
        {
            string fileName = name + fileExtension;
            if (old_Key == null || !fileName.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (Manager.Get(Site, fileName) != null)
                {
                    return Json("The name already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult Sort(string directoryPath, IEnumerable<string> filesOrder)
        {
            Manager.SaveOrder(Site, filesOrder);
            return null;
        }

        public virtual ActionResult Localize(string fileName)
        {
            Manager.Localize(Site, fileName);

            return RedirectToIndex();
        }
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
        public virtual void Export(Kooboo.CMS.Sites.Models.ScriptFile[] model)
        {
            var fileName = "Scripts.zip";
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
