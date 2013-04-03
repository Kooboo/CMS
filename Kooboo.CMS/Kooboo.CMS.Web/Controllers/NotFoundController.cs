using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Controllers
{
    public class NotFoundController : Controller
    {
        //
        // GET: /NotFound/

        public virtual ActionResult Index()
        {
            return View();
        }
    }
}
