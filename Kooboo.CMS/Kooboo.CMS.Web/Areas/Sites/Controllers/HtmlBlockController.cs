#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;
using Kooboo.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "HtmlBlock", Order = 1)]
    public class HtmlBlockController : PathResourceControllerBase<Kooboo.CMS.Sites.Models.HtmlBlock, HtmlBlockManager>
    {
        public HtmlBlockController(HtmlBlockManager manager)
            : base(manager) { }

        #region CURD
        public override ActionResult Create(HtmlBlock model)
        {
            ViewBag.ExternalCssSetting = this.GetExternalCssSetting();
            return base.Create(model);
        }

        public override ActionResult Edit(string uuid)
        {
            ViewBag.ExternalCssSetting = this.GetExternalCssSetting();
            return base.Edit(uuid);
        }

        private string GetExternalCssSetting()
        {
            if (!string.IsNullOrEmpty(Site.Theme))
            {
                string cssHackBody;
                var themefiles = Kooboo.CMS.Sites.Parsers.ThemeRule.ThemeRuleParser.Parse(new Theme(Site, Site.Theme).LastVersion(), out cssHackBody);
                var files = themefiles.Where(o => o.PhysicalPath.EndsWith(".css", StringComparison.CurrentCultureIgnoreCase))
                                      .Select(o => Kooboo.Web.Url.UrlUtility.ResolveUrl(o.VirtualPath)).ToList();
                return string.Join(",", files);
            }
            else
            {
                return "";
            }
        }

        public virtual ActionResult Localize(HtmlBlock[] model)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                foreach (var item in model)
                {
                    Manager.Localize(item.UUID, Site);
                }
                data.ReloadPage = true;
            });
            return Json(data);
        }


        public virtual ActionResult IsNameAvailable(string name, string old_Key)
        {
            if (old_Key == null || !name.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (Manager.Get(Site, name) != null)
                {
                    return Json("The name already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #endregion

        #region Relations
        public virtual ActionResult Relations(string uuid)
        {
            var model = Manager.Relations(new HtmlBlock() { Site = Site, UUID = uuid });
            return View("Relations", model);
        }
        #endregion

        #region Version
        public virtual ActionResult Versions(string uuid)
        {
            var htmlBlock = new HtmlBlock(Site, uuid);
            var model = VersionManager.AllVersions<HtmlBlock>(htmlBlock);
            return View(model);
        }

        public virtual ActionResult Revert(string uuid, int version, string @return)
        {
            var data = new JsonResultData(ModelState);
            var htmlBlock = new HtmlBlock(Site, uuid);

            data.RunWithTry((resultData) =>
            {
                VersionManager.Revert<HtmlBlock>(htmlBlock, version);
                data.RedirectUrl = @return;
            });
            return Json(data);
        }

        public virtual ActionResult PreviewVersion(string uuid, int version)
        {
            var htmlBlock = new HtmlBlock(Site, uuid);
            var model = VersionManager.GetVersion<HtmlBlock>(htmlBlock, version);
            return View(model);
        }
        #endregion

        #region Import/Export
        #region Import
        public virtual ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Import(bool @override, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    Manager.Provider.Import(Site, Request.Files[0].InputStream, @override);
                }
                data.RedirectUrl = @return;
            });
            return Json(data, "text/plain", System.Text.Encoding.UTF8);
        }
        #endregion

        #region Export
        [HttpPost]
        public virtual void Export(Kooboo.CMS.Sites.Models.HtmlBlock[] model)
        {
            var fileName = "HtmlBlocks.zip";
            Response.AttachmentHeader(fileName);
            if (model != null)
            {
                foreach (var item in model)
                {
                    item.Site = Site;
                }
            }
            Manager.Provider.Export(Site, model, Response.OutputStream);
        }
        #endregion
        #endregion
    }
}
