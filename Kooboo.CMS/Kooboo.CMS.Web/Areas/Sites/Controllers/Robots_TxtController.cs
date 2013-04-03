using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;
using Kooboo.CMS.Sites;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
     [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Robots.txt", Order = 1)]
    public class Robots_TxtController : AdminControllerBase
    {
        //
        // GET: /Sites/Robot_Txt/

        public virtual ActionResult Index()
        {
            Robots_Txt robot_txt = new Robots_Txt(Site);

            var robotStr = robot_txt.Read();

            robot_txt.Body = robotStr;
            return View(robot_txt);
        }

        [HttpPost]
        public virtual ActionResult Index(string body)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                Robots_Txt robot_txt = new Robots_Txt(Site);
                robot_txt.Save(body);
                resultEntry.AddMessage("The robots.txt has been saved.".Localize());
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);
        }

    }
}
