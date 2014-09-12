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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Modules.DiscardDraft
{
    public class PageDraftController : AreaControllerBase
    {
        private PageManager _pageManager;
        public PageDraftController(PageManager pageManager)
        {
            this._pageManager = pageManager;
        }
        public ActionResult Discard(string uuid, string @return)
        {
            ModelState.Clear();
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                var page = new Page(Site, uuid);

                _pageManager.Provider.RemoveDraft(page);

                data.RedirectUrl = @return;
            });

            return Json(data);
        }
    }
}
