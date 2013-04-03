using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Pages", Name = "Publish", Order = 1)]
    public class PagePublishingQueueController : AdminControllerBase
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
            var result = new JsonResultEntry();
            try
            {
                foreach (var item in model)
                {
                    item.Site = Site;
                    ServiceFactory.PageManager.PagePublishingProvider.Remove(item);
                }
                result.SetSuccess();
            }
            catch (Exception e)
            {
                result.SetFailed().AddException(e);
            }

            return Json(result);
        }

    }
}
