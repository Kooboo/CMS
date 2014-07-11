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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Pages", Name = "Publish", Order = 1)]
    public class PagePublishingQueueController : Kooboo.CMS.Sites.AreaControllerBase
    {
        //
        // GET: /Sites/PagePublishingQueue/
        public virtual ActionResult Index()
        {
            var items = ServiceFactory.PageManager.PagePublishingProvider.All(Site).Select(it => it.AsActual());
            return View(items);
        }

        public virtual ActionResult Delete(PagePublishingQueueItem[] model)
        {
            var result = new JsonResultData(ModelState);

            result.RunWithTry((resultEntry) =>
            {
                foreach (var item in model)
                {
                    item.Site = Site;
                    ServiceFactory.PageManager.PagePublishingProvider.Remove(item);
                }
            });            

            return Json(result);
        }

    }
}
