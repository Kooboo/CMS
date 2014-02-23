using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KoobooModule.Areas.KoobooModule.Controllers
{
    [Kooboo.CMS.Sites.InitializeCurrentContext]
    public class ApplicationManagementController : Controller
    {
        //
        // GET: /KoobooModule/ApplicationManagement/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Recycle")]
        public ActionResult RecyclePost()
        {
            System.Web.Hosting.HostingEnvironment.InitiateShutdown();
            return RedirectToAction("Index");
        }

    }
}
