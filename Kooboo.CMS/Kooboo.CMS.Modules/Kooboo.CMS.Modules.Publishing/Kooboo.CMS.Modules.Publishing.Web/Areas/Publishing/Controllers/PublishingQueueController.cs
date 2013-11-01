using Kooboo.CMS.Common;
using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Services;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Controllers
{
    public class PublishingQueueController : AreaControllerBase
    {
        #region .ctor
        private readonly PublishingQueueManager _manager;
        public PublishingQueueController(PublishingQueueManager manager)
        {
            this._manager = manager;
        } 
        #endregion

        #region Index
        public ActionResult Index(string siteName, string search,int? publishingObject,int? publishingType,int? status,
            string sortField,string sortDir)
        {
            var query = this._manager.CreateQuery(siteName);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(it => it.ObjectUUID.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            if (publishingObject.HasValue)
            {
                PublishingObject po = (PublishingObject)publishingObject.Value;
                query = query.Where(it => it.PublishingObject == po);
            }
            if (publishingType.HasValue)
            {
                PublishingType pt = (PublishingType)publishingType.Value;
                query = query.Where(it => it.PublishingType == pt);
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

        #region Create
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(PublishingQueue queue, string @return)
        {
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                queue.UserId = User.Identity.Name;
                if (queue.UtcTimeToPublish.HasValue)
                {
                    queue.UtcTimeToPublish = queue.UtcTimeToPublish.Value.ToUniversalTime();
                }
                if (queue.UtcTimeToUnpublish.HasValue)
                {
                    queue.UtcTimeToUnpublish = queue.UtcTimeToUnpublish.Value.ToUniversalTime();
                }
                queue.UtcCreationDate = DateTime.UtcNow;
                resultEntry.RunWithTry((data) =>
                {
                    _manager.Add(queue);
                    data.RedirectUrl = @return;
                });
            }
            return Json(resultEntry);
        } 
        #endregion

        #region Detail
        public ActionResult Detail(string uuid)
        {
            var model = this._manager.Get(uuid);
            return View(model);
        } 
        #endregion

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
                        _manager.Delete(uuids);
                    }
                    data.ReloadPage = true;
                });
            }
            return Json(resultEntry);
        } 
        #endregion
    }
}
