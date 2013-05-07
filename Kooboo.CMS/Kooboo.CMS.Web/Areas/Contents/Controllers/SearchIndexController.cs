#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
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
using Kooboo.CMS.Common;
using Kooboo.CMS.Search.Models;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    public class SearchIndexController : ManagerControllerBase
    {
        IndexSummaryService IndexSummaryService { get; set; }
        public SearchIndexController(IndexSummaryService indexSummaryService)
        {
            IndexSummaryService = indexSummaryService;
        }
        #region Index
        public virtual ActionResult Index()
        {
            var viewModel = new IndexSummaryModel()
            {
                FolderIndexInfoes = IndexSummaryService.GetFolderIndexInfoes(Repository),
                LastActions = IndexSummaryService.GetLastActions(Repository)
            };
            return View(viewModel);
        } 
        #endregion
        #region Rebuild
        public virtual ActionResult Rebuild(FolderIndexInfo[] model, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                foreach (var item in model)
                {
                    var textFolder = new TextFolder(Repository, item.FolderName);
                    IndexSummaryService.Rebuild(textFolder);
                }
                data.RedirectUrl = @return;
            });

            return Json(data);
        } 
        #endregion
    }
}
