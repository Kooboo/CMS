using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
     [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Settings", Order = 1)]
    public class DiagnosisController : AdminControllerBase
    {
        public virtual ActionResult Index()
        {
            return View(ServiceFactory.SystemManager.Diagnosis(Site.AsActual()));
        }
    }
}
