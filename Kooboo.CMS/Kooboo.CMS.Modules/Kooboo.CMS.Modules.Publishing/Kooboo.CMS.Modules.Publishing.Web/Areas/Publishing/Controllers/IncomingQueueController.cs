using Kooboo.CMS.Common;
using Kooboo.CMS.Modules.Publishing.Services;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Publishing", Group = "Remote", Name = "Incoming", Order = 1)]
    public class IncomingQueueController : AreaControllerBase
    {
        #region .ctor
        private readonly IncomingQueueManager _manager;

        public IncomingQueueController(IncomingQueueManager manager)
        {
            this._manager = manager;
        }
        #endregion

        #region Index
        public ActionResult Index(string search)
        {
            var query = this._manager.CreateQuery(Site);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(it => it.Vendor.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            return View(query.ToList());
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete(DeleteModel[] model)
        {
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                var uuids = model.Select(it => it.UUID).ToArray();
                this._manager.Delete(Site, uuids);
                resultEntry.ReloadPage = true;
            }
            return Json(resultEntry);
        }
        #endregion
    }
}
