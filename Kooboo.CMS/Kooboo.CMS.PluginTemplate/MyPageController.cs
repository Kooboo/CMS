using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.PluginTemplate
{
    public class MyPageController : Controller
    {
        public ActionResult Entry()
        {
            return Content("MyPageController");
        }
    }
}
