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
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;
using Kooboo.CMS.Form;
using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.Globalization;
using Kooboo.Web;
using Kooboo.Web.Mvc;
using Kooboo.CMS.Web;
using Kooboo.CMS.Web.Models;
namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Schema")]
    [StateControllerContextActionFilterAttribute]
    public class ContentTypeController : ManagerControllerBase
    {
        #region .ctor
        public ContentTypeController(SchemaManager manager)
        {
            Manager = manager;
        }

        SchemaManager Manager { get; set; }
        #endregion

        #region Index
        [HttpGet]
        public virtual ActionResult Index(string search, string schemaName, string sortField, string sortDir)
        {
            return View(Manager.All(Repository, search).AsQueryable().SortBy(sortField, sortDir));
        }

        #endregion

        #region Create
        [HttpGet]
        public virtual ActionResult Create()
        {
            ViewBag.AllSchemas = Manager.All(Repository, "");
            return View();
        }

        [HttpPost]
        public virtual ActionResult Create(Schema schema)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    Manager.Add(Repository, schema);

                    this.ResetForm(schema);

                    resultData.RedirectUrl = ControllerContext.RequestContext.GetRequestValue("return");
                });
            }
            return Json(data);
        }
        #endregion

        #region Edit
        [HttpGet]
        public virtual ActionResult Edit(string uuid)
        {
            return View(Manager.Get(Repository, uuid).AsActual());
        }

        [HttpPost]
        public virtual ActionResult Edit(Schema newModel)
        {
            var data = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
                {
                    var oldModel = Manager.Get(Repository, newModel.Name);
                    Manager.Update(Repository, newModel, oldModel);
                    ResetForm(newModel);
                    resultData.RedirectUrl = Request["return"];
                });
            }
            return Json(data);
        }
        #endregion

        #region Delete
        [HttpPost]
        public virtual ActionResult Delete(Schema[] model)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        Manager.Remove(Repository, new Schema(Repository, item.Name));
                    }
                    resultData.ReloadPage = true;
                }
            });
            return Json(data);
        }
        #endregion

        #region ResetTemplates
        public virtual ActionResult ResetTemplates(Schema[] model)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        ResetForm(item);
                    }
                    data.AddMessage("The templates have been reset.".Localize());
                }
            });

            return Json(data);
        }
        private void ResetForm(Schema schema)
        {
            Manager.ResetForm(Repository, schema.Name, FormType.Create | FormType.Detail | FormType.Grid | FormType.List | FormType.Selectable | FormType.Update);
        }
        #endregion

        #region Import&Export Schema
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
                var file = Request.Files[0];
                Manager.Import(Repository, file.InputStream, @override);
                data.RedirectUrl = @return;
            });
            return Json(data);
        }

        public virtual ActionResult Export(Schema[] model)
        {
            if (model != null && model.Count() > 0)
            {
                string fileName = "Schema.zip";
                Response.AttachmentHeader(fileName);
                Manager.Export(Repository, Response.OutputStream, model);
                return null;
            }
            else
            {
                return RedirectToIndex();
            }

        }

        #endregion

        #region Relations
        public virtual ActionResult Relations(string uuid)
        {
            var model = Manager.GetRelationFolders(new Schema { Name = uuid, Repository = Repository })
                .Select(o => new RelationModel()
            {
                RelationName = o.FriendlyName,
                RelationType = "Folder".Localize()
            });
            return View(model);
        }
        #endregion

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(string name, string old_Key)
        {
            if (old_Key == null || !name.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (Manager.Get(Repository, name) != null)
                {
                    return Json("The name already exists.".Localize(), JsonRequestBehavior.AllowGet);
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
                Manager.Copy(Repository, copyModel.UUID, copyModel.DestinationName);
                data.RedirectUrl = @return;
            });

            return Json(data);
        }

        public virtual ActionResult CopyNameAvailabled(CopyModel copyModel)
        {
            return this.IsNameAvailable(copyModel.DestinationName, null);
        }


        #endregion

    }
}
