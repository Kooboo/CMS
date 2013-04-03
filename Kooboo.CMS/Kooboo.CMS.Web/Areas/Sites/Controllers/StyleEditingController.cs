using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    using Kooboo.Globalization;
    using Kooboo.CMS.Sites;
    using Kooboo.CMS.Sites.Services;
    using Kooboo.CMS.Web.Authorizations;
    using Kooboo.CMS.Content.Models;
    using Kooboo.CMS.Sites.Models;

    [Authorization(AreaName = "Sites", Group = "Page", Name = "Style editing", Order = 1)]
    public class StyleEditingController : AdminControllerBase
    {
        public virtual ActionResult FrontEnd(string siteName, bool? _draft_)
        {
            var manager = ServiceFactory.GetService<ThemeFileManager>();
            var files = manager.GetFiles(Site, Site.Theme)
                               .Where(o => o.PhysicalPath.EndsWith(".css", StringComparison.CurrentCultureIgnoreCase))
                               .Select(o => o.VirtualPath).ToList();
            // params
            ViewBag.CssFiles = files;
            ViewBag.SiteName = siteName;
            ViewBag._draft_ = _draft_;
            // ret
            return View();
        }

        [HttpPost]
        public virtual ActionResult SaveFile(string siteName, string filePath, string fileContent, bool? _draft_)
        {
            var result = new JsonResultEntry();
            try
            {
                // save content
                filePath = Server.MapPath(filePath);
                System.IO.File.WriteAllText(filePath, fileContent, System.Text.Encoding.UTF8);
                // update version
                string styleVersion = null;
                var versionPrefix = string.Empty;
                var version = this.Site.Version.Trim();
                for (var i = version.Length - 1; i > -1; i--)
                {
                    var charAt = version[i];
                    if (char.IsNumber(charAt))
                    {
                        styleVersion = charAt + styleVersion;
                    }
                    else
                    {
                        versionPrefix = version.Substring(0, i);
                        break;
                    }
                }
                styleVersion = styleVersion ?? "0";
                styleVersion = (int.Parse(styleVersion) + 1).ToString();
                this.Site.Version = versionPrefix + "." + styleVersion;
                ServiceFactory.SiteManager.Update(this.Site);
            }
            catch (Exception e)
            {
                result.AddException(e);
            }
            // ret
            return Json(result);
        }

        public virtual ActionResult Localization()
        {
            var json = PageDesignController.jsSerializer.Serialize(new
            {
                selectors_js = new
                {
                    title = "CSS Rules".Localize(),
                    searchTitle = "Search".Localize(),
                    searchTitleFormat = "Search ({0})items".Localize(),
                    searchInputTip = "Case insensitive and use whitespace to separate multiple keys.".Localize(),
                    btnSearch = "Go".Localize(),
                    btnClear = "X".Localize(),
                    btnSearchTitle = "Search".Localize(),
                    btnClearTitle = "Clear".Localize(),
                    changeTitle = "changed".Localize(),
                    noReadyMsg = "Initialization is processing, please wait.".Localize()
                },

                ruletext_js = new
                {
                    title = "Rule text".Localize(),
                    moveTitle = "move up/down".Localize(),
                    btnSwapTitle = "edit all".Localize(),
                    btnSwapBackTitle = "save".Localize(),
                    statusWarningTitle = "warning".Localize(),
                    statusEnableTitle = "enable".Localize(),
                    statusDisabledTitle = "disabled".Localize(),
                    titleExpended = "expended".Localize(),
                    titleCollapsed = "collapsed".Localize(),
                    defaultMessage = "Select a css rule to edit here.".Localize()
                },

                preview_js = new
                {
                    title = "Preview".Localize(),
                    sampleText = "Change styles from editor to see effects here.(Visual Style Editor)".Localize()
                },

                editors_js = new
                {
                    title = "Editors".Localize(),
                    fontGroup = "Font properties".Localize(),
                    textGroup = "Text properties".Localize(),
                    boxGroup = "Box properties".Localize(),
                    positioningGroup = "Positioning properties".Localize(),
                    backgroundGroup = "Background properties".Localize(),
                    base_js = new
                    {
                        enable = "enable".Localize(),
                        disabled = "disabled".Localize(),
                        important = "important".Localize(),
                        noImportant = "no important".Localize()
                    },
                    file_js = new
                    {
                        btnTitle = "image library".Localize(),
                        imgLibTitle = "Media library".Localize(),
                        mediaLibraryUrl = Url.Action("Selection", new { controller = "MediaContent", repositoryName = Repository.Current.Name, siteName = Site.Current.FullName, area = "Contents" })
                    },
                    unit_js = new
                    {
                        unitTitle = "unit".Localize()
                    }
                }
            });

            // ret
            return JavaScript(string.Format("var __localization = {0};", json));
        }

        public virtual ActionResult FrontVariables(string siteName, bool? _draft_)
        {
            var json = PageDesignController.jsSerializer.Serialize(new
            {
                front_js = new
                {
                    editUrl = Url.Action("FrontEnd", "StyleEditing", new { siteName = siteName, _draft_ = _draft_ }),
                    dialogTitle = "Style Editor".Localize(),
                    btnAbleTitle = "Open Style Editor".Localize()
                }
            });

            // ret
            return JavaScript(string.Format("var __localization = {0};", json));
        }
    }
}
