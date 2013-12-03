using Kooboo.CMS.Common;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Kooboo.Web.Mvc;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Models;

namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Controllers
{
    [InitializeCurrentContext]
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Blog", Group = "", Name = "Category", Order = 1)]
    public class CategoryController : Controller
    {
        IProvider<Category> _provider;
        public CategoryController(IProvider<Category> provider)
        {
            this._provider = provider;
        }
        #region Index
        public virtual ActionResult Index()
        {
            return View(_provider.CreateQuery().Where(it => it.SiteName == Site.Current.FullName));
        }
        #endregion

        #region Create
        [HttpGet]
        public virtual ActionResult Create()
        {
            return View();
        }
        [ValidateAntiForgeryToken]
        [HttpPost]
        public virtual ActionResult Create(Category model, string @return)
        {
            JsonResultData data = new JsonResultData(ViewData.ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _provider.Add(model);
                    resultData.RedirectUrl = @return;
                });
            }

            return Json(data);
        }
        #endregion

        #region Edit
        public virtual ActionResult Edit(int id)
        {
            var model = _provider.QueryById(id);
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Edit(int id, string @return)
        {
            JsonResultData data = new JsonResultData(ViewData.ModelState);

            var model = _provider.QueryById(id);
            this.UpdateModel(model);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    _provider.Update(model);
                    resultData.RedirectUrl = @return;
                });
            }

            return Json(data);
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete(Category[] model)
        {
            JsonResultData data = new JsonResultData();
            data.RunWithTry((resultData) =>
            {
                foreach (var item in model)
                {
                    _provider.Delete(item);
                }
                data.RedirectUrl = Url.Action("Index", ControllerContext.RequestContext.AllRouteValues().Merge("id", null));
            });
            return Json(data);

        }
        #endregion
    }
}
