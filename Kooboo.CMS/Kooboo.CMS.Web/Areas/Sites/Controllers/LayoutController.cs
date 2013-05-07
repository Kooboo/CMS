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
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.Globalization;
using Kooboo.Web.Mvc;
using Kooboo.Web;
using Kooboo.Web.Script.Serialization;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Common;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "Layout", Order = 1)]
    public class LayoutController : PathResourceControllerBase<Layout, LayoutManager>
    {
        #region .ctor
        public LayoutController(LayoutManager manager)
            : base(manager)
        {
        }
        #endregion

        #region Create
        public override ActionResult Create()
        {
            var engineName = ControllerContext.RequestContext.GetRequestValue("EngineName");
            ViewData["LayoutSamples"] = ServiceFactory.LayoutItemTemplateManager.GetLayoutSamples(engineName);
            ViewData["ViewEngine"] = Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(engineName);
            ViewBag.DefaultLayout = Kooboo.CMS.Sites.Services.ServiceFactory.LayoutItemTemplateManager.GetDefaultLayoutSample(engineName);
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public override ActionResult Create(Layout model, string @return)
        {
            var data = new JsonResultData(ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    model.UserName = User.Identity.Name;
                    Add(model);
                    resultData.RedirectUrl = @return;
                });
            }

            return Json(data);
            //return base.Create(model);
        }
        #endregion

        #region Edit
        public override ActionResult Edit(string uuid)
        {
            var layout = Manager.Get(Site, uuid);
            ViewData["LayoutSamples"] = ServiceFactory.LayoutItemTemplateManager.GetLayoutSamples(layout.EngineName);
            ViewData["ViewEngine"] = Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(layout.EngineName);
            ViewBag.DefaultLayout = Kooboo.CMS.Sites.Services.ServiceFactory.LayoutItemTemplateManager.GetDefaultLayoutSample(layout.EngineName);
            return View(layout);
        }

        [HttpPost]
        [ValidateInput(false)]
        public override ActionResult Edit(Layout newModel, string old_key, string @return)
        {
            var data = new JsonResultData(ModelState);

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    newModel.UserName = User.Identity.Name;

                    Update(newModel, old_key);

                    data.AddMessage("The item has been saved.".Localize());
                });
            }

            return Json(data);
        }
        #endregion

        #region Relations
        public virtual ActionResult Relations(string uuid)
        {
            var model = Manager.RelationsPages(new Layout() { Site = Site, UUID = uuid }).Select(o => new RelationModel
            {
                RelationName = o.FriendlyName,
                RelationType = "Page".Localize()
            });
            return View("Relations", model);
        }
        #endregion

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(string name, string old_Key)
        {
            if (old_Key == null || !name.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (Manager.Get(Site, name) != null)
                {
                    return Json("The name already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Copy
        public virtual ActionResult Copy(string uuid)
        {
            return View(new CopyModel() { UUID = uuid });
        }
        [HttpPost]
        public virtual ActionResult Copy(CopyModel copyModel, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    Manager.Copy(Site, copyModel.UUID, copyModel.DestinationName);
                    data.RedirectUrl = @return;
                }
            });
            return Json(data);

        }

        public virtual ActionResult CopyNameAvailabled(CopyModel copyModel)
        {
            return this.IsNameAvailable(copyModel.DestinationName, null);
        }

        #endregion

        #region Localize
        public virtual ActionResult Localize(Layout[] model)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                foreach (var item in model)
                {
                    Manager.Localize(item.UUID, Site);
                }
                data.ReloadPage = true;
            });
            return Json(data);
        }
        #endregion

        #region Versions
        public virtual ActionResult Versions(string uuid)
        {
            var layout = new Layout(Site, uuid);
            var versions = Kooboo.CMS.Sites.Versioning.VersionManager.AllVersions(layout);
            ViewBag.Name = uuid;
            return View(versions);
        }
        public virtual ActionResult PreviewVersion(string uuid, int version)
        {
            var layout = Manager.Get(Site, uuid);

            var versionedLayout = Kooboo.CMS.Sites.Versioning.VersionManager.GetVersion(layout, version);

            ViewData["LayoutSamples"] = ServiceFactory.LayoutItemTemplateManager.GetLayoutSamples(layout.EngineName);
            ViewData["ViewEngine"] = Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(layout.EngineName);
            return View(versionedLayout);
        }

        public virtual ActionResult Revert(string uuid, int version, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                var layout = new Layout(Site, uuid);
                Kooboo.CMS.Sites.Versioning.VersionManager.Revert(layout, version);
                resultData.RedirectUrl = @return;
            });

            return Json(data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Import/Export
        #region Import
        public virtual ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Import(bool @override, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    Manager.Provider.Import(Site, Request.Files[0].InputStream, @override);
                }
                data.RedirectUrl = @return;
            });
            return Json(data, "text/plain", System.Text.Encoding.UTF8);
        }
        #endregion

        #region Export
        [HttpPost]
        public virtual void Export(Kooboo.CMS.Sites.Models.Layout[] model)
        {
            var fileName = "Layouts.zip";
            Response.AttachmentHeader(fileName);
            foreach (var item in model)
            {
                item.Site = Site;
            }
            Manager.Provider.Export(model, Response.OutputStream);
        }
        #endregion
        #endregion

    }
}
