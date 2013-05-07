#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Web.Mvc;
using Kooboo.CMS.ModuleArea.Models;
using Kooboo.CMS.ModuleArea.Repositories;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Common;

namespace Kooboo.CMS.ModuleArea.Controllers
{
    public class NewsAdminController : AdminControllerBase
    {
        #region .ctor
        IRepository<News> repository;
        public NewsAdminController(IRepository<News> repository)
        {
            this.repository = repository;
        } 
        #endregion

        #region Index
        public ActionResult Index()
        {
            return View(repository.All());
        } 
        #endregion

        #region Create
        public ActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public ActionResult Create(News news, string @return)
        {
            JsonResultData resultEntry = new JsonResultData(ViewData.ModelState);
            try
            {
                if (ModelState.IsValid)
                {
                    repository.Add(news);
                    resultEntry.RedirectUrl = @return;
                }

            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }

            return Json(resultEntry);
        } 
        #endregion

        #region Edit
        public ActionResult Edit(int id)
        {
            var news = repository.ById(id);
            return View(news);
        }


        [HttpPost]
        public ActionResult Edit(int id, News news, string @return)
        {
            JsonResultData resultEntry = new JsonResultData(ViewData.ModelState);
            try
            {
                if (ModelState.IsValid)
                {
                    repository.Update(news);
                    resultEntry.RedirectUrl = @return;
                }

            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }

            return Json(resultEntry);
        } 
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete(News[] model)
        {
            JsonResultData resultEntry = new JsonResultData();
            try
            {
                foreach (var item in model)
                {
                    repository.Delete(item);
                }
                resultEntry.RedirectUrl = Url.Action("Index", ControllerContext.RequestContext.AllRouteValues().Merge("id", null));
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);

        } 
        #endregion
    }
}
