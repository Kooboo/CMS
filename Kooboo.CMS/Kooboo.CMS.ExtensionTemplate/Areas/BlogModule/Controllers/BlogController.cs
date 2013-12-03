
using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

using Kooboo.Web.Mvc;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Persistence;
using Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Models;
using Kooboo.CMS.Sites;
namespace Kooboo.CMS.ExtensionTemplate.Areas.BlogModule.Controllers
{
    [InitializeCurrentContext]
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Blog", Group = "", Name = "Blog", Order = 1)]
    public class BlogController : Controller
    {
        #region .ctor
        IProvider<Blog> _provider;
        IProvider<Category> _categoryProvider;
        public BlogController(IProvider<Blog> provider, IProvider<Category> categoryProvider)
        {
            this._provider = provider;
            _categoryProvider = categoryProvider;
        }
        #endregion

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
        public virtual ActionResult Create([Bind(Exclude = "Categories")]Blog model, string @return, int[] categories = null)
        {
            JsonResultData data = new JsonResultData(ViewData.ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    SetCategories(model, categories);
                    _provider.Add(model);
                    resultData.RedirectUrl = @return;
                });
            }

            return Json(data);
        }
        #endregion

        private void SetCategories(Blog model, int[] categories)
        {
            if (categories != null && categories.Length > 0)
            {
                var categoryModels = _categoryProvider.CreateQuery().Where(it => categories.Contains(it.Id)).ToArray();
                if (model.Categories == null)
                {
                    model.Categories = categoryModels;
                }
                else
                {

                    var removed = model.Categories.Where(it => !categories.Contains(it.Id)).ToArray();
                    foreach (var item in removed)
                    {
                        model.Categories.Remove(item);
                    }

                    foreach (var item in categoryModels)
                    {
                        if (!model.Categories.Any(it => item.Id == it.Id))
                        {
                            model.Categories.Add(item);
                        }
                    }
                }

            }
            else
            {
                if (model.Categories != null)
                {
                    model.Categories.Clear();
                }
            }
        }

        #region Edit
        public virtual ActionResult Edit(int id)
        {
            var model = _provider.QueryById(id);
            return View(model);
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Edit(int id, string @return, int[] categories = null)
        {
            JsonResultData data = new JsonResultData(ViewData.ModelState);

            var model = _provider.QueryById(id);
            this.UpdateModel(model, "", null, new[] { "Categories" });

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    SetCategories(model, categories);
                    _provider.Update(model);
                    resultData.RedirectUrl = @return;
                });
            }

            return Json(data);
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete(Blog[] model)
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
