using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Sites;

namespace Kooboo.CMS.Web.Areas.Account.Controllers
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 1)]
    public class SettingController : ControllerBase
    {
        //
        // GET: /Account/Setting/

        public ActionResult Index()
        {
            return View(Kooboo.CMS.Account.Persistence.RepositoryFactory.SettingRepository.Get());
        }
        [HttpPost]
        public ActionResult Index(Setting setting)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                Kooboo.CMS.Account.Persistence.RepositoryFactory.SettingRepository.Update(setting, setting);
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }

            return Json(resultEntry);
        }
    }
}
