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
using Kooboo.Web.Mvc;
using Kooboo.Web;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Common;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "PageMapping", Order = 1)]
    public class UrlKeyMapController : ManageControllerBase<UrlKeyMap, UrlKeyMapManager>
    {
        public UrlKeyMapController(UrlKeyMapManager manager) : base(manager) { }

        #region import/export

        protected string GetZipFileName()
        {
            return "UrlKeyMaps.zip";
        }
        public virtual ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Import(bool @override, string @return)
        {
            JsonResultData resultEntry = new JsonResultData(ModelState);
            try
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    Manager.Import(Site, Request.Files[0].InputStream, @override);
                }
                resultEntry.RedirectUrl = @return;
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

        public virtual ActionResult IsKeyAvailable(string key, string old_Key)
        {
            if (old_Key == null || !key.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (ServiceFactory.UrlKeyMapManager.Get(Site, key) != null)
                {
                    return Json("The key is already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
