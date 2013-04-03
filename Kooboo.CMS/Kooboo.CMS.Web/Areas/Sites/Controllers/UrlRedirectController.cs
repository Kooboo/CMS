using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.Models;
using Kooboo.Web.Mvc;
using Kooboo.Web;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Url redirect", Order = 1)]
    public class UrlRedirectController : IManagerControllerBase<UrlRedirect, UrlRedirectManager>
    {
        #region import/export

        protected string GetZipFileName()
        {
            return "UrlRedirects.zip";
        }

        public virtual ActionResult Import(bool @override)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    Manager.Import(Site, Request.Files[0].InputStream, @override);
                }
                resultEntry.ReloadPage = true;
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry, "text/plain", System.Text.Encoding.UTF8);

        }

        public virtual ActionResult Export()
        {
            var fileName = GetZipFileName();
            Response.AttachmentHeader(fileName);
            Manager.Export(Site, Response.OutputStream);
            return null;
        }


        #endregion
        public virtual ActionResult IsInputUrlAvailable(string inputUrl, string old_key)
        {
            if (old_key == null || !inputUrl.EqualsOrNullEmpty(old_key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (ServiceFactory.UrlRedirectManager.Get(Site, inputUrl) != null)
                {
                    return Json("The input url/pattern is already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
