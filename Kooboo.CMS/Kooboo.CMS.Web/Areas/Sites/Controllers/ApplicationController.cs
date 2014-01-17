using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    public class ApplicationController : Controller
    {
        //
        // GET: /Sites/ApplicationManagment/

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