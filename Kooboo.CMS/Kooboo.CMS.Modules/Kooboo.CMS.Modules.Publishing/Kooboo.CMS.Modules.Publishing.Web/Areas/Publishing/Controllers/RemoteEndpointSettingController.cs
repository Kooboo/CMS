using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common;

using Kooboo.CMS.Sites;
using Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Kooboo.CMS.Modules.Publishing.Services;
using Kooboo.CMS.Modules.Publishing.Cmis;
using Kooboo.CMS.Modules.Publishing.Models;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Publishing", Group = "Remote", Name = "RemoteSites", Order = 1)]
    public class RemoteEndpointSettingController : AreaControllerBase
    {
        private readonly RemoteSettingManager _manager;
        private readonly ICmisSession _cmisSession;
        public RemoteEndpointSettingController(RemoteSettingManager manager, ICmisSession cmisSession)
        {
            this._manager = manager;
            this._cmisSession = cmisSession;
        }

        public ActionResult Index(string siteName, string search, string sortField, string sortDir)
        {
            var query = this._manager.CreateQuery(siteName);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(it => it.Name.Contains(search));
            }
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                query = query.SortByField(sortField, sortDir);
            }
            return View(query.ToList());
        }

        public ActionResult Create()
        {
            var model = new RemoteEndpointSetting();
            model.Enabled = true;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RemoteEndpointSetting setting, string @return)
        {
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                resultEntry.RunWithTry((data) =>
                {
                    _manager.Add(setting);
                    data.RedirectUrl = @return;
                });
            }
            return Json(resultEntry);
        }

        public ActionResult Edit(string uuid)
        {
            var model = _manager.Get(uuid);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RemoteEndpointSetting setting, string @return)
        {
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                resultEntry.RunWithTry((data) =>
                {
                    var oldModel = _manager.Get(setting.UUID);
                    _manager.Update(setting, oldModel);
                    resultEntry.RedirectUrl = @return;
                });
            }
            return Json(resultEntry);
        }

        [HttpPost]
        public ActionResult Delete(DeleteModel[] model)
        {
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                resultEntry.RunWithTry((data) =>
                {
                    var uuids = model.Select(it => it.UUID).ToArray();
                    if (uuids.Any())
                    {
                        _manager.Delete(uuids);
                    }
                    data.ReloadPage = true;
                });
            }
            return Json(resultEntry);
        }

        [HttpPost]
        public ActionResult GetRemoteRepositories(string cmisService, string cmisUserName, string cmisPassword)
        {
            var resultEntry = new JsonResultData(ModelState);
            resultEntry.RunWithTry((data) =>
            {
                var items = this._cmisSession.OpenSession(cmisService, cmisUserName, cmisPassword).GetRepositories();
                data.Model = items.Select(it => new
                {
                    id = it.Key,
                    text = it.Value
                }).ToArray();
            });

            return Json(resultEntry);
        }
    }
}
