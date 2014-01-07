using Kooboo.CMS.Modules.Publishing.Models;
using Kooboo.CMS.Modules.Publishing.Services;
using Kooboo.CMS.Sites;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Publishing", Group = "Local", Name = "Logs", Order = 1)]
    public class PublishingLogController : AreaControllerBase
    {
        private readonly PublishingLogManager _manager;
        public PublishingLogController(PublishingLogManager manager)
        {
            this._manager = manager;
        }

        public ActionResult Index(string siteName,string search,int? queueType,int? publishingObject,int? publishingType,int? status,
            string sortField, string sortDir)
        {
            var query = this._manager.CreateQuery(siteName);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(it => it.ObjectUUID.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            if (queueType.HasValue)
            {
                QueueType qt = (QueueType)queueType.Value;
                query = query.Where(it => it.QueueType == qt);
            }
            if (publishingObject.HasValue)
            {
                PublishingObject po = (PublishingObject)publishingObject.Value;
                query = query.Where(it => it.PublishingObject == po);
            }
            //if (publishingType.HasValue)
            //{
            //    PublishingType pt = (PublishingType)publishingType.Value;
            //    query = query.Where(it => it.PublishingType == pt);
            //}
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
                query = query.OrderByDescending(it => it.UtcProcessedTime);
            }
            return View(query.ToList());
        }

    }
}
