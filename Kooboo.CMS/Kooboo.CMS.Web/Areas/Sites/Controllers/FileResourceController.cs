using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Globalization;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
        [Kooboo.CMS.Web.Authorizations.LargeFileAuthorization(AreaName = "Sites", Group = "Development", Name = "File", Order = 1)]
    public class FileResourceController : FileManagerControllerBase
    {
        public FileResourceController() :
            base(new CustomFileManagerEx())
        {

        }
        public override ActionResult Index(string directoryPath)
        {
            ViewData["Title"] = "Files".Localize();
            return base.Index(directoryPath);
        }
    }
}
