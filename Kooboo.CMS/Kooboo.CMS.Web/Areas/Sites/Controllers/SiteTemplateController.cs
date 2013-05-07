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
using Kooboo.CMS.Web.Areas.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 99)]
    public class SiteTemplateController : AreaControllerBase
    {
        #region .ctor
        public SiteTemplateManager SiteTemplateManager { get; set; }
        public SiteTemplateController(SiteTemplateManager siteTemplateManager)
        {
            SiteTemplateManager = siteTemplateManager;
        }
        #endregion

        #region Index
        public ActionResult Index(string search)
        {
            var list = SiteTemplateManager.All();
            if (!string.IsNullOrEmpty(search))
            {
                list = list.Where(it => it.TemplateName.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            return View(list);
        }
        #endregion

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(SiteTemplate_ViewModel model)
        {
            if (SiteTemplateManager.GetItemTemplate(model.Category, model.TemplateName) != null)
            {
                return Json("The name already exists.", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Upload
        public ActionResult Upload()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Upload(SiteTemplate_ViewModel model, string @return)
        {
            ViewData.ModelState.Remove("TemplateFile");
            ViewData.ModelState.Remove("ThumbnailFile");
            var data = new JsonResultData(ViewData.ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    SiteTemplateManager.AddItemTemplate(model.Category, model.TemplateName, model.TemplateFile.InputStream, model.ThumbnailFile == null ? null : model.ThumbnailFile.InputStream);
                    resultData.RedirectUrl = @return;
                });
            }

            return Json(data);
        }
        #endregion

        #region Delete
        [HttpPost]
        public virtual ActionResult Delete(ItemTemplate[] model)
        {
            ModelState.Clear();
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        SiteTemplateManager.DeleteItemTemplate(item);
                    }
                }

                data.ReloadPage = true;
            });

            return Json(data);

        }
        #endregion

    }
}
