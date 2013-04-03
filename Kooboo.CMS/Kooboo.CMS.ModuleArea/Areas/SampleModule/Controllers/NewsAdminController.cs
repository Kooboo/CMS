using System;
using System.Web.Mvc;
using Kooboo.CMS.ModuleArea.Models;
using Kooboo.CMS.ModuleArea.Repositories;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.Web.Mvc;

namespace Kooboo.CMS.ModuleArea.Controllers
{
    public class NewsAdminController : AdminControllerBase
    {
        IRepository<News> repository;
        public NewsAdminController(IRepository<News> repository)
        {
            this.repository = repository;
        }
        public NewsAdminController()
            : this(new NewsRepository())
        {
        }

        //
        // GET: /News/

        public ActionResult Index()
        {
            return View(repository.All());
        }


        //
        // GET: /Default1/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Default1/Create

        [HttpPost]
        public ActionResult Create(News news)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ViewData.ModelState);
            try
            {
                if (ModelState.IsValid)
                {
                    repository.Add(news);
                    resultEntry.RedirectUrl = Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues());
                }

            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }

            return Json(resultEntry);
        }

        //
        // GET: /Default1/Edit/5

        public ActionResult Edit(int id)
        {
            var news = repository.ById(id);
            return View(news);
        }

        //
        // POST: /Default1/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, News news)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ViewData.ModelState);
            try
            {
                if (ModelState.IsValid)
                {
                    repository.Update(news);
                    resultEntry.RedirectUrl = Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues());
                }

            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }

            return Json(resultEntry);
        }

        //
        // POST: /Default1/Delete/5

        [HttpPost]
        public ActionResult Delete(News[] model)
        {
            JsonResultEntry resultEntry = new JsonResultEntry();
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
    }
}
