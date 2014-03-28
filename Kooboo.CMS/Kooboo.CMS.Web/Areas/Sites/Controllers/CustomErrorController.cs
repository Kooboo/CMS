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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Web;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Common;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Custom error", Order = 1)]
    public class CustomErrorController : ManageControllerBase<CustomError, CustomErrorManager>
    {
        #region .ctor
        public CustomErrorController(CustomErrorManager manager)
            : base(manager)
        {
        }
        #endregion

        #region Methods

        #region import/export
        protected string GetZipFileName()
        {
            return "CustomErrors.zip";
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


        public ActionResult Export(CustomError[] model)
        {
            var fileName = GetZipFileName();
            Response.AttachmentHeader(fileName);
            if (model != null)
            {
                foreach (var item in model)
                {
                    item.Site = Site;
                }
            }
            Manager.Export(Site, model, Response.OutputStream);
            return null;
        }

        #endregion

        public ActionResult IsStatusCodeAvailable(string statusCode, string old_Key)
        {
            var enumValue = Enum.Parse(typeof(HttpErrorStatusCode), statusCode);
            var enumString = enumValue.ToString();
            if (!enumString.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (ServiceFactory.CustomErrorManager.Get(Site, enumString) != null)
                {
                    return Json("The status code is already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion
    }
}
