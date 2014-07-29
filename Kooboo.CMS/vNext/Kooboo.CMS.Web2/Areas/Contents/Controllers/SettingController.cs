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
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.Web;
using Kooboo.Common.Globalization;
using Kooboo.CMS.Web2.Models;
using Kooboo.CMS.Sites;
using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Web2.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web2.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Setting")]
    public class SettingController : ManagerControllerBase
    {
        #region .ctor
        RepositoryManager Manager { get; set; }
        public SettingController(RepositoryManager manager)
        {
            Manager = manager;
        }
        #endregion

        #region Index
        [HttpGet]
        public virtual ActionResult Index()
        {
            return View(Repository);
        }
        public virtual ActionResult Index(Repository model)
        {
            JsonResultData data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    Manager.Update(model, Manager.Get(model.Name));
                    data.ReloadPage = true;
                }
            });
            return Json(data);
        }
        #endregion
    }
}
