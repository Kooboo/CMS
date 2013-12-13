using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.VSExtensionTemplate.Areas.HelloWorld.Controllers
{
    [InitializeCurrentContext]
    [Kooboo.CMS.Sites.Extension.ModuleArea.ModuleContextInitialize]
    public class AdminController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}