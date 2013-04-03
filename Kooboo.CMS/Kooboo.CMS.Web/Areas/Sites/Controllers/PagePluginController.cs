using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    public class PagePluginController : Controller
    {
        //
        // GET: /Admin/PagePlugins/

        public virtual ActionResult Index()
        {
            return View();
        }

    }
}
