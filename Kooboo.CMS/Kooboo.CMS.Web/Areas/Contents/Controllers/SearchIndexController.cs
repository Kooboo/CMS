using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Search;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Content.Models;
using Kooboo.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    public class SearchIndexController : ManagerControllerBase
    {
        //
        // GET: /Contents/SearchIndex/

        public virtual ActionResult Index()
        {
            var viewModel = new IndexSummaryModel()
            {
                FolderIndexInfoes = IndexSummaryService.Instance.GetFolderIndexInfoes(Repository),
                LastActions = IndexSummaryService.Instance.GetLastActions(Repository)
            };
            return View(viewModel);
        }
        public virtual ActionResult Rebuild(string folderName)
        {
            var entry = new JsonResultEntry();
            try
            {
                var textFolder = new TextFolder(Repository, folderName);
                IndexSummaryService.Instance.Rebuild(textFolder);
                entry.RedirectUrl = Url.Action("Index", Request.RequestContext.AllRouteValues());
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }
    }
}
