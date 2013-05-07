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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Web.Mvc;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    public abstract class PathResourceControllerBase<T, Service> : ManageControllerBase<T, Service>
        where T : Kooboo.CMS.Sites.Models.PathResource
        where Service : IManager<T>
    {
        #region .ctor
        public PathResourceControllerBase(Service manager) : base(manager) { }
        #endregion

        #region Update
        protected override void Update(T newModel, string old_key)
        {
            var oldModel = Manager.Get(Site, old_key);

            Manager.Update(Site, newModel, oldModel);
        }
        #endregion

        #region Unlocalize
        [HttpPost]
        public virtual ActionResult Unlocalize(T o, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                Remove(o);
                resultData.ReloadPage = true;
            });

            return Json(data);
        }
        #endregion
    }
}