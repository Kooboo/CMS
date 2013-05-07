#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.ABTest;
using Kooboo.CMS.Web.Authorizations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.Web;
using Kooboo.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "System", Name = "A/B Test", Order = 1)]
    public class ABRuleSettingController : ManageControllerBase<ABRuleSetting, VisitRuleSettingManager>
    {
        #region .ctor
        VisitRuleSettingManager _manager;
        public ABRuleSettingController(VisitRuleSettingManager manager)
            : base(manager)
        {
            _manager = manager;
        }
        #endregion

        #region import/export
        public void Export(ABRuleSetting[] model)
        {
            //var fullNameArray = model.Select(o => o.FullName);
            //var selected = Manager.All(Site, "").Where(o => fullNameArray.Contains(o.FullName));

            var fileName = GetZipFileName();
            Response.AttachmentHeader(fileName);
            foreach (var item in model)
            {
                item.Site = Site;
            }
            Manager.Export(model, Response.OutputStream);
        }

        protected string GetZipFileName()
        {
            return "VisitRules.zip";
        }

        public virtual ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Import(bool @override, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    Manager.Import(Site, Request.Files[0].InputStream, @override);
                }
                data.RedirectUrl = @return;
            });
            return Json(data, "text/plain", System.Text.Encoding.UTF8);
        }

        #endregion

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(string name, string old_Key)
        {
            if (old_Key == null || !name.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (Manager.Get(Site, name) != null)
                {
                    return Json("The name already exists.".Localize(), JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Get
        public ActionResult GetJson(string uuid)
        {
            var rule = Get(uuid);

            return Json(rule.RuleItems);
        }
        #endregion

    }
}
