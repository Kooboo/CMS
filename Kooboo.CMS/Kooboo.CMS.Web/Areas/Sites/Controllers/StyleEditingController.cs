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
using Kooboo.Web.Script.Serialization;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    using Kooboo.Globalization;
    using Kooboo.CMS.Sites;
    using Kooboo.CMS.Sites.Services;
    using Kooboo.CMS.Web.Authorizations;
    using Kooboo.CMS.Content.Models;
    using Kooboo.CMS.Sites.Models;
    using Kooboo.CMS.Common;

    [Authorization(AreaName = "Sites", Group = "Page", Name = "Style editing", Order = 1)]
    public class StyleEditingController : Kooboo.CMS.Sites.AreaControllerBase
    {
        public virtual ActionResult FrontEnd(string siteName, bool? _draft_)
        {
            var manager = ServiceFactory.GetService<ThemeManager>();
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
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
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
            });
            return Json(data);
        }

        public virtual ActionResult Localization()
        {
            var json = (new
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
                    changeTitle = "Changed".Localize(),
                    noReadyMsg = "Initialization is processing, please wait.".Localize()
                },

                ruletext_js = new
                {
                    title = "Rule text".Localize(),
                    moveTitle = "Move up/down".Localize(),
                    btnSwapTitle = "Edit all".Localize(),
                    btnSwapBackTitle = "Save".Localize(),
                    statusWarningTitle = "Warning".Localize(),
                    statusEnableTitle = "Enable".Localize(),
                    statusDisabledTitle = "Disabled".Localize(),
                    titleExpended = "Expended".Localize(),
                    titleCollapsed = "Collapsed".Localize(),
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
                        enable = "Enable".Localize(),
                        disabled = "Disabled".Localize(),
                        important = "Important".Localize(),
                        noImportant = "no important".Localize()
                    },
                    file_js = new
                    {
                        btnTitle = "Image library".Localize(),
                        imgLibTitle = "Media library".Localize(),
                        mediaLibraryUrl = Url.Action("Selection", new { controller = "MediaContent", repositoryName = Repository.Current.Name, siteName = Site.Current.FullName, area = "Contents" })
                    },
                    unit_js = new
                    {
                        unitTitle = "Unit".Localize()
                    }
                }
            }).ToJSON();

            // ret
            return JavaScript(string.Format("var __localization = {0};", json));
        }

        public virtual ActionResult FrontVariables(string siteName, bool? _draft_)
        {
            var json = (new
            {
                front_js = new
                {
                    editUrl = Url.Action("FrontEnd", "StyleEditing", new { siteName = siteName, _draft_ = _draft_ }),
                    dialogTitle = "Style Editor".Localize(),
                    btnAbleTitle = "Open Style Editor".Localize()
                }
            }).ToJSON();

            // ret
            return JavaScript(string.Format("var __localization = {0};", json));
        }
    }
}
