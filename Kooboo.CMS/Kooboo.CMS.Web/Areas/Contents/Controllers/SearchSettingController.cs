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
using Kooboo.CMS.Search.Models;
using Kooboo.CMS.Search;
using Kooboo.CMS.Search.Persistence;
using Kooboo.CMS.Search.Persistence.Caching;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Common;
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Models;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "SearchSetting")]
    public class SearchSettingController : ManagerControllerBase
    {
        #region .ctor
        ISearchSettingProvider Provider
        {
            get;
            set;
        }
        public SearchSettingController(ISearchSettingProvider provider)
        {
            Provider = provider;
        }
        #endregion

        #region Index
        public virtual ActionResult Index(string sortField, string sortDir)
        {
            var list = Provider.All(Repository).AsQueryable().SortBy(sortField, sortDir);
            list = list.Select(o => GetEntity(o));
            return View(list);
        }
        SearchSetting GetEntity(SearchSetting model)
        {
            var r = Provider.Get(new SearchSetting(Repository, model.Name));
            r.Name = model.Name;
            return r;
        }
        #endregion

        #region Create
        public virtual ActionResult Create()
        {
            return View(new SearchSetting());
        }

        [HttpPost]
        public virtual ActionResult Create(SearchSetting model, string @return)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    model.Repository = Repository;
                    if (Provider.Get(model) != null)
                    {
                        resultData.AddErrorMessage("The item already exists.".Localize());
                    }
                    else
                    {
                        Provider.Add(model);
                        resultData.RedirectUrl = @return;
                    }
                });
            }
            return Json(data);
        }
        #endregion

        #region Edit
        public virtual ActionResult Edit(string uuid)
        {
            var model = Provider.Get(new SearchSetting { UUID = uuid, Repository = Repository });
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Edit(SearchSetting model, string @return)
        {
            var data = new JsonResultData(ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    model.Repository = Repository;
                    Provider.Update(model, model);
                    resultData.RedirectUrl = @return;
                });
            }
            return Json(data);
        }
        #endregion

        #region Delete
        [HttpPost]
        public virtual ActionResult Delete(SearchSetting[] model)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                if (model != null)
                {
                    foreach (var m in model)
                    {
                        m.Repository = Repository;
                        Provider.Remove(m);
                    }
                }
            });

            return Json(data);
        }
        #endregion

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(string folderName)
        {
            if (!string.IsNullOrEmpty(folderName))
            {
                if (Provider.Get(new SearchSetting(Repository, folderName)) != null)
                {
                    return Json("The folder search setting already exists.", JsonRequestBehavior.AllowGet);
                }
            }


            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region GetPageRouteFields
        [HttpPost]
        public ActionResult GetPageRouteFields(string pageName)
        {
            JsonResultData result = new JsonResultData();
            result.RunWithTry((jsonData) =>
            {
                var page = new Page(Site.Current, pageName).AsActual();
                if (page != null && page.Route != null && page.Route.Defaults != null)
                {
                    jsonData.Model = page.Route.Defaults.Select(it => new { Key = it.Key, Value = it.Value }).ToArray();
                }
            });
            return Json(result);
        }
        #endregion

    }
}
