#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Models;
using Kooboo.Web;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Url redirect", Order = 1)]
    public class UrlRedirectController : ManageControllerBase<UrlRedirect, UrlRedirectManager>
    {
        #region .ctor
        public UrlRedirectController(UrlRedirectManager manager)
            : base(manager)
        { }
        #endregion

        #region Methods
        public override ActionResult Index(string search, string sortField, string sortDir, int? page, int? pageSize)
        {
            return View(List(search, sortField, sortDir).ToPagedList(page ?? 1, pageSize ?? 50));
        }
        #region import/export

        protected string GetZipFileName()
        {
            return "UrlRedirects.zip";
        }
        public ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Import(bool @override, string @return)
        {
            JsonResultData data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    Manager.Import(Site, Request.Files[0].InputStream, @override);
                }
                data.RedirectUrl = @return;
            });
            return Json(data, "text/plain", System.Text.Encoding.UTF8);

        }

        public ActionResult Export()
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
                if (ServiceFactory.UrlRedirectManager.GetByInputUrl(Site, inputUrl) != null)
                {
                    return Json("The input url/pattern is already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
