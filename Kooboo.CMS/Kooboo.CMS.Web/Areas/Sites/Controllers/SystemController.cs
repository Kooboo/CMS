#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.Common.Globalization;
using Kooboo.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "Settings", Order = 1)]
    public class SystemController : Kooboo.CMS.Sites.AreaControllerBase
    {
        #region Diagnosis
        public virtual ActionResult Diagnosis()
        {
            return View(ServiceFactory.SystemManager.Diagnosis(Site.AsActual()));
        }
        #endregion
    }
}
