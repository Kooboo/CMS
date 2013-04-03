using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "User", Order = 1)]
    public class UsersController : IManagerControllerBase<User, UserManager>
    {
        protected override IEnumerable<User> List(string search)
        {
            return base.List(search).Select(it => it.AsActual());
        }     

        public virtual ActionResult IsUserAvailable(string userName)
        {
            if (Kooboo.CMS.Account.Services.ServiceFactory.UserManager.Get(userName) == null)
            {
                return Json("The user is not available.", JsonRequestBehavior.AllowGet);
            }
            if (Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.Get(Site, userName) != null)
            {
                return Json("The user is already exists.", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
