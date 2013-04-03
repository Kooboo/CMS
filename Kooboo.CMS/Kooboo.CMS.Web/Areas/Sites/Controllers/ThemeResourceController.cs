using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Sites;
using System.IO;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.LargeFileAuthorization(AreaName = "Sites", Group = "Development", Name = "Theme", Order = 1)]
    public class ThemeResourceController : FileManagerControllerBase
    {
        public ThemeResourceController()
            : base(ServiceFactory.GetService<ThemeFileManager>())
        {

        }
        public override ActionResult Index(string directoryPath)
        {
            ViewData["Title"] = "Themes".Localize();

            var themeName = ControllerContext.RequestContext.GetRequestValue("ThemeName");

            if (!string.IsNullOrEmpty(themeName))
            {
                var theme = new Theme(Site, themeName);
                ViewBag.Theme = theme;
            }

            return base.Index(directoryPath);
        }
        public virtual ActionResult ChangeHeaderBackground(string themeName)
        {
            var theme = new Theme(Site, themeName);
            ViewBag.VirtualPath = ServiceFactory.HeaderBackgroundManager.GetVirutalPath(theme);
            ViewBag.ContainerSize = ServiceFactory.HeaderBackgroundManager.GetContainerSize(Site);
            return View();
        }
        [HttpPost]
        public virtual ActionResult SaveBackground(string themeName, string Url, int Width, int Height, int x, int y)
        {
            var theme = new Theme(Site, themeName);
            ServiceFactory.HeaderBackgroundManager.Change(theme, Url, x, y);
            var filePath = Server.MapPath(Url);
            if (System.IO.File.Exists(filePath))
            {
                System.IO.File.Delete(filePath);
            }
            return Json(true);
        }

        [HttpPost]
        public virtual ActionResult PostFile()
        {
            var entry = new JsonResultEntry();

            try
            {

                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    var postFile = Request.Files[0];
                    var tempPath = Kooboo.Web.Url.UrlUtility.Combine(Site.VirtualPath, "Temp");
                    Kooboo.IO.IOUtility.EnsureDirectoryExists(Server.MapPath(tempPath));

                    var fileUrl = Kooboo.Web.Url.UrlUtility.Combine(tempPath, Guid.NewGuid() + Path.GetFileName(postFile.FileName));

                    postFile.SaveAs(Server.MapPath(fileUrl));
                    entry.Model = Url.Content(fileUrl);
                }
                else
                {
                    entry.SetFailed().AddMessage("It is not a valid image file.".Localize());
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

    }
}
