#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using Kooboo.Globalization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Common.Persistence;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    public abstract class ManageControllerBase<T, Service> : Kooboo.CMS.Sites.AreaControllerBase
  where Service : IManager<T>
    {
        #region .ctor
        protected Service Manager
        {
            get;
            private set;
        }
        public ManageControllerBase(Service manager)
        {
            Manager = manager;
        }
        #endregion

        #region Index
        public virtual ActionResult Index(string search, string sortField, string sortDir, int? page, int? pageSize)
        {
            var list = List(search, sortField, sortDir);
            return View(list);
        }
        protected virtual IEnumerable<T> List(string search, string sortField, string sortDir)
        {
            var list = Manager.All(Site, search);
            list = list.AsQueryable().SortBy(sortField, sortDir);
            return list;
        }
        #endregion

        #region Create
        public virtual ActionResult Create(T model)
        {
            ModelState.Clear();
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult Create(T model, string @return)
        {
            var data = new JsonResultData(ModelState) { RedirectUrl = @return };
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    Add(model);
                });
            }
            return Json(data);

        }
        protected virtual void Add(T model)
        {
            if (model is IChangeTimeline)
            {
                var changeTimeline = (IChangeTimeline)model;

                changeTimeline.UtcCreationDate = DateTime.UtcNow;
                changeTimeline.LastestEditor = User.Identity.Name;
            }
            Manager.Add(Site, model);
        }
        #endregion

        #region Edit
        public virtual ActionResult Edit(string uuid)
        {
            var o = Get(uuid);
            return View(o);
        }


        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult Edit(T newModel, string uuid, string @return)
        {
            var data = new JsonResultData(ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    Update(newModel, uuid);
                    resultData.RedirectUrl = @return;
                });
            }
            return Json(data);
        }
        protected virtual T Get(string name)        {
            
            return Manager.Get(Site, name);
        }
        protected virtual void Update(T newModel, string old_key)
        {
            if (newModel is IChangeTimeline)
            {
                var changeTimeline = (IChangeTimeline)newModel;

                changeTimeline.UtcLastestModificationDate = DateTime.UtcNow;
                changeTimeline.LastestEditor = User.Identity.Name;
            }
            Manager.Update(Site, newModel, Manager.Get(Site, old_key));
        }
        #endregion

        #region Delete

        [HttpPost]
        public virtual ActionResult Delete(T[] model)
        {
            ModelState.Clear();
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (model != null)
                {
                    foreach (var t in model)
                    {
                        Remove(t);
                    }
                }

                data.ReloadPage = true;
            });

            return Json(data);

        }
        protected virtual void Remove(T model)
        {
            Manager.Remove(Site, model);
        }
        #endregion
    }
}