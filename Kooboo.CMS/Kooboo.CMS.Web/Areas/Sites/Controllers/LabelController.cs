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
using Kooboo.CMS.Sites.Services;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Globalization;
using System.Web.Routing;
using Kooboo.Web.Mvc;
using Kooboo.Extensions;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites;
using Kooboo.Web;
using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Models;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "Label", Order = 1)]
    public class LabelController : Kooboo.CMS.Sites.AreaControllerBase
    {
        #region Manager

        public LabelManager Manager
        {
            get;
            set;
        }
        public LabelController(LabelManager manager)
        {
            Manager = manager;
        }

        #endregion

        #region Index,Edit
        public virtual ActionResult Index(string category, string search, int? page, int? pageSize, string sortField, string sortDir)
        {
            var queryable = Manager.All(Site, category);
            if (!string.IsNullOrEmpty(search))
            {
                search = search.Trim();
                queryable = queryable.Where(it => it.Name.Contains(search, StringComparison.CurrentCultureIgnoreCase) || it.Value.Contains(search, StringComparison.CurrentCultureIgnoreCase));
            }

            var pagedList = queryable.SortBy(sortField, sortDir).ToPagedList<Label>(page ?? 1, pageSize ?? 50);

            if (string.IsNullOrEmpty(category))
            {
                ViewData["CategoryList"] = Manager.GetCategories(Site);
            }

            return View(pagedList);
        }

        #endregion

        #region Edit
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Edit(Label model, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                Update(model);
                resultData.RedirectUrl = @return;
            });
            return Json(data);
        }
        protected virtual void Update(Label model)
        {
            var newModel = Manager.Get(Site, model.Category, model.Name);
            newModel.UtcLastestModificationDate = DateTime.UtcNow;
            newModel.LastestEditor = User.Identity.Name;
            newModel.Value = model.Value;
            Manager.Update(Site, newModel, newModel);
        }
        #endregion


        #region Delete

        public virtual ActionResult Delete(string[] docs, string[] folders)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                var category = ControllerContext.RequestContext.GetRequestValue("Category");

                if (docs != null)
                {
                    foreach (var uuid in docs)
                    {
                        var label = new Label() { Category = category, Site = Site, UUID = uuid };
                        Remove(label);
                    }
                }


                if (folders != null)
                {
                    foreach (var c in folders)
                    {
                        Manager.RemoveCategory(Site, c);
                    }
                }

                resultData.ReloadPage = true;
            });
            return Json(data);
        }
        protected virtual void Remove(Label model)
        {
            Manager.Remove(Site, model);
        }
        #endregion

        #region Category

        public virtual ActionResult RemoveCategory(string category)
        {
            Manager.RemoveCategory(Site, category);
            if (!string.IsNullOrEmpty(category))
            {
                var index = category.LastIndexOf('.');
                if (index != -1)
                {
                    return RedirectToIndex(category.Substring(0, index));
                }
            }
            return RedirectToIndex("");
        }
        protected ActionResult RedirectToIndex()
        {
            return RedirectToIndex("");
        }
        protected virtual ActionResult RedirectToIndex(string category)
        {
            var routeValues = this.ControllerContext.RequestContext.AllRouteValues();
            if (!string.IsNullOrEmpty(category))
            {
                routeValues["category"] = category;
            }
            return RedirectToAction("Index", routeValues);
        }
        #endregion

        #region UpgradeFromOldLabel
        [HttpPost]
        public virtual ActionResult UpgradeFromOldLabel()
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                Manager.UpgradeFromOldLabel(Site);
                resultData.ReloadPage = true;
            });
            return Json(data);
        }
        #endregion

        #region Import/Export
        public void Export(string category, string[] docs, string[] folders)
        {
            var fileName = GetZipFileName();
            Response.AttachmentHeader(fileName);
            IEnumerable<Label> labels = new Label[0];
            if (docs != null)
            {
                labels = docs.Select(it => new Label(Site, it) { Category = category });
            }
            Manager.Export(Site, labels, folders, Response.OutputStream);
        }

        protected string GetZipFileName()
        {
            return "Labels.zip";
        }

        public ActionResult Import()
        {
            return View(Kooboo.CMS.Web.Models.ImportModel.Default);
        }
        [HttpPost]
        public virtual ActionResult Import(bool @override, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    Manager.Import(Site, Request.Files[0].InputStream, @override);
                }
                resultData.RedirectUrl = @return;
            });
            return Json(data, "text/plain", System.Text.Encoding.UTF8);
        }

        #endregion

    }
}
