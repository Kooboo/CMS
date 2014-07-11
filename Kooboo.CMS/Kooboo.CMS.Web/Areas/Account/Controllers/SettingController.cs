#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Account.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Account.Persistence;
using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Globalization;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Web.Areas.Account.Controllers
{
    [Kooboo.CMS.Web.Authorizations.RequiredLogOnAttribute(RequiredAdministrator = true, Order = 1)]
    public class SettingController : ControllerBase
    {
        public ISettingProvider SettingProvider { get; private set; }
        public SettingController(ISettingProvider settingProvider)
        {
            this.SettingProvider = settingProvider;
        }

        public ActionResult Index()
        {
            return View(SettingProvider.Get());
        }
        [HttpPost]
        public ActionResult Index(Setting setting)
        {
            JsonResultData data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                SettingProvider.Update(setting, setting);
                resultData.AddMessage("The setting has been saved.".Localize());
            });

            return Json(data);
        }
    }
}
