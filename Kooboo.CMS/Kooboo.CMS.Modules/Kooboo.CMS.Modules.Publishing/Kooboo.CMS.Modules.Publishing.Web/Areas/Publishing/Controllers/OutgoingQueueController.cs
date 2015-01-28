using Kooboo.CMS.Common;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Services;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Controllers
{
    public class OutgoingQueueController : AreaControllerBase
    {
        #region .ctor
        private readonly OutgoingQueueManager _manager;
        private readonly PublishingLogManager _outgoingLogManager;
        public OutgoingQueueController(OutgoingQueueManager manager, PublishingLogManager outgoingLogManager)
        {
            this._manager = manager;
            this._outgoingLogManager = outgoingLogManager;
        } 
        #endregion

        #region Index
        public ActionResult Index(string siteName, string search, int? publishingObject, int? status,
            string sortField,string sortDir)
        {
            var query = this._manager.CreateQuery(Site);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(it => it.ObjectUUID.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            if (publishingObject.HasValue)
            {
                PublishingObject po = (PublishingObject)publishingObject.Value;
                query = query.Where(it => it.PublishingObject == po);
            }
            if (status.HasValue)
            {
                QueueStatus qs = (QueueStatus)status;
                query = query.Where(it => it.Status == qs);
            }
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                query = query.SortByField(sortField, sortDir);
            }
            else
            {
                query = query.OrderByDescending(it => it.UtcCreationDate);
            }
            return View(query.ToList());
        } 
        #endregion

        #region Logs
        public ActionResult Logs(string siteName, string uuid,string sortField,string sortDir)
        {
            var query = this._outgoingLogManager.CreateQuery(Site).Where(it => it.QueueType==QueueType.Outgoing&&it.ObjectUUID.Equals(uuid, StringComparison.OrdinalIgnoreCase));
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                query = query.SortByField(sortField, sortDir);
            }
            return View(query.ToList());
        } 
        #endregion

        public ActionResult Edit(string uuid)
        {
            var model = _manager.Get(Site,uuid);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(string uuid,QueueStatus status, string @return)
        {
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                resultEntry.RunWithTry((data) =>
                {
                    var oldModel = _manager.Get(Site,uuid);
                    var newModel = _manager.Get(Site,uuid);
                    newModel.Status = status;
                    _manager.Update(newModel, oldModel);
                    resultEntry.RedirectUrl = @return;
                });
            }
            return Json(resultEntry);
        }

        #region Delete
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
                        _manager.Delete(Site,uuids);
                    }
                    data.ReloadPage = true;
                });
            }
            return Json(resultEntry);
        } 
        #endregion
    }
}
