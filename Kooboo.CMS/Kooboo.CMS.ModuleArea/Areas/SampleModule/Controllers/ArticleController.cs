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
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Search;
namespace Kooboo.CMS.ModuleArea.Areas.SampleModule.Controllers
{
    public class ArticleController : ModuleControllerBase
    {
        public ActionResult Categories()
        {
            var repository = Repository.Current;
            var categoryFolder = new TextFolder(repository, "Category");

            return View(categoryFolder.CreateQuery());
        }

        public ActionResult List(string userKey, int? pageIndex, int? pageSize)
        {
            var repository = Repository.Current;
            var categoryFolder = new TextFolder(repository, "Category");
            var articleFolder = new TextFolder(repository, "Article");

            var articleQuery = articleFolder.CreateQuery();

            //var userKey = Page_Context.Current.PageRequestContext.AllQueryString["UserKey"];

            if (!string.IsNullOrEmpty(userKey) && userKey != "--")
            {
                articleQuery = articleQuery.WhereCategory(categoryFolder.CreateQuery().WhereEquals("UserKey", userKey));
            }

            if (!pageIndex.HasValue)
            {
                pageIndex = 1;
            }
            if (!pageSize.HasValue)
            {
                pageSize = 10;
            }
            var pageData = articleQuery.Skip((pageIndex.Value - 1) * pageSize.Value).Take(pageSize.Value);

            DataRulePagedList pagedList = new DataRulePagedList(pageData,
                pageIndex.Value, pageSize.Value, articleQuery.Count());
            return View(pagedList);
        }


        public ActionResult Search(string key, int? pageIndex, int? pageSize)
        {
            if (!pageIndex.HasValue)
            {
                pageIndex = 1;
            }
            if (!pageSize.HasValue)
            {
                pageSize = 10;
            }
            var results = Repository.Current.Search(key, pageIndex.Value, pageSize.Value);

            return View(results);
        }
    }
}
