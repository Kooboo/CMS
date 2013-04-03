using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Models;
using Kooboo.Globalization;
using Kooboo.Web;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "HtmlBlock", Order = 1)]
    public class HtmlBlockController : PathResourceControllerBase<Kooboo.CMS.Sites.Models.HtmlBlock, HtmlBlockManager>
    {
        #region CURD
        public override ActionResult Create()
        {
            ViewBag.ExternalCssSetting = this.GetExternalCssSetting();
            return base.Create();
        }

        public override ActionResult Edit(string name)
        {
            ViewBag.ExternalCssSetting = this.GetExternalCssSetting();
            return base.Edit(name);
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

        public virtual ActionResult Localize(string name)
        {
            Manager.Localize(name, Site);
            return RedirectToIndex();
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

        #region Version
        public virtual ActionResult Version(string name)
        {
            var htmlBlock = new HtmlBlock(Site, name);
            var model = Manager.VersiongLogger.AllVersions(htmlBlock);
            return View(model);
        }

        public virtual ActionResult Revert(string name, int version)
        {
            var entry = new JsonResultEntry();
            var htmlBlock = new HtmlBlock(Site, name);

            try
            {
                Manager.VersiongLogger.Revert(htmlBlock, version);
                entry.SetSuccess().AddMessage("Revert Successfully.".Localize());
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

        public virtual ActionResult PreviewVersion(string name, int version)
        {
            var page = new HtmlBlock(Site, name);
            var model = Manager.VersiongLogger.GetVersion(page, version);
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
        public virtual void Export(Kooboo.CMS.Sites.Models.HtmlBlock[] model)
        {
            var fileName = "HtmlBlocks.zip";
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
