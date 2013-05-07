#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "User", Order = 1)]
    public class UsersController : ManageControllerBase<User, UserManager>
    {
        public UsersController(UserManager manager) : base(manager) { }
      
        public virtual ActionResult IsUserAvailable(string userName)
        {
            if (Kooboo.CMS.Account.Services.ServiceFactory.UserManager.Get(userName) == null)
            {
                return Json("The user is not available.", JsonRequestBehavior.AllowGet);
            }
            if (Kooboo.CMS.Sites.Services.ServiceFactory.UserManager.Get(Site, userName) != null)
            {
                return Json("The user already exists.", JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}
