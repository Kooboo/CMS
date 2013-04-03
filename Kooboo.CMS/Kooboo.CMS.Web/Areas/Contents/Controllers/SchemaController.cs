using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Services;

using Kooboo.Web;
using Kooboo.Web.Script.Serialization;
using Kooboo.Web.Mvc;

using Kooboo.CMS.Web.Areas.Contents.Models;
using Kooboo.Globalization;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Form;
using Kooboo.CMS.Sites;
//using Kooboo.CMS.Web.ErrorHandling;

namespace Kooboo.CMS.Web.Areas.Contents.Controllers
{

    public class ValidationBinder : DefaultModelBinder
    {
        protected override object CreateModel(ControllerContext controllerContext, ModelBindingContext bindingContext, Type modelType)
        {
            if (modelType == typeof(ColumnValidation))
            {
                var validationType = (ValidationType)bindingContext.ValueProvider.GetValue(bindingContext.ModelName + ".ValidationType").ConvertTo(typeof(Kooboo.CMS.Form.ValidationType));
                object model = null;
                switch (validationType)
                {
                    case ValidationType.Required:
                        model = new RequiredValidation();
                        break;
                    case ValidationType.StringLength:
                        model = new StringLengthValidation();
                        break;
                    case ValidationType.Range:
                        model = new RangeValidation();
                        break;
                    case ValidationType.Regex:
                        model = new RegexValidation();
                        break;
                }
                bindingContext.ModelMetadata = ModelMetadataProviders.Current.GetMetadataForType(() => model, model.GetType());
                return model;
            }

            return base.CreateModel(controllerContext, bindingContext, modelType);
        }
        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var result = base.BindModel(controllerContext, bindingContext);

            return result;

        }
    }

    //Kooboo.CMS.Account.Models.Permission.Contents_SchemaPermission
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Contents", Group = "", Name = "Schema")]
    public class SchemaController : ManagerControllerBase
    {
        static SchemaController()
        {
            ModelBinders.Binders.Add(typeof(ColumnValidation), new ValidationBinder());
        }
        public SchemaController()
        {
            Manager = ServiceFactory.SchemaManager;
        }

        SchemaManager Manager { get; set; }

        #region Schema
        //
        // GET: /Content/Schema/
        [HttpGet]
        public virtual ActionResult Index(string search, string schemaName, bool? Finish)
        {
            //if (Finish.HasValue && Finish.Value == true)
            //{
            //    ResetAll(schemaName);
            //}

            return View(Manager.All(Repository, search));
        }

        [HttpGet]
        public virtual ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Create(Schema schema, bool? finish)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                if (ModelState.IsValid)
                {
                    Manager.Add(Repository, schema);

                    this.ResetAllForm(schema.Name);

                    if (finish.Value == true)
                    {
                        resultEntry.RedirectUrl = this.Url.Action("Index", Request.RequestContext.AllRouteValues().Merge("schemaName", schema.Name));
                    }
                    else
                    {
                        resultEntry.RedirectUrl = this.Url.Action("Templates", Request.RequestContext.AllRouteValues().Merge("schemaName", schema.Name));
                    }
                }

            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);
        }

        [HttpGet]
        public virtual ActionResult Edit(string schemaName)
        {
            return View(Manager.Get(Repository, schemaName).AsActual());
        }
        [HttpPost]
        public virtual ActionResult Edit(Schema newModel, string old_key, bool? finish, bool Next, bool resetAll)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                if (ModelState.IsValid)
                {
                    var oldModel = Manager.Get(Repository, old_key);
                    Manager.Update(Repository, newModel, oldModel);

                    if (resetAll)
                    {
                        ResetAllForm(newModel.Name);
                    }
                    if (finish.Value == true)
                    {
                        resultEntry.RedirectUrl = this.Url.Action("Index", Request.RequestContext.AllRouteValues().Merge("schemaName", newModel.Name));
                    }
                    else
                    {
                        resultEntry.RedirectUrl = this.Url.Action("Templates", Request.RequestContext.AllRouteValues().Merge("schemaName", newModel.Name));
                    }
                    //}
                }

            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);
        }

        [HttpPost]
        public virtual ActionResult Delete(Schema[] model)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        Manager.Remove(Repository, new Schema(Repository, item.Name));
                    }
                }
            }
            catch (Exception e)
            {
                entry.SetFailed().AddException(e);
            }

            return Json(entry);
        }

        public virtual ActionResult ResetTemplates(Schema[] model)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                if (model != null)
                {
                    foreach (var item in model)
                    {
                        ResetAllForm(item.Name);
                    }
                    entry.AddMessage("The templates have been reset.".Localize());
                }
            }
            catch (Exception e)
            {
                entry.SetFailed().AddException(e);
            }

            return Json(entry);
        }

        #region Import&Export Schema

        public virtual ActionResult Import(bool @override)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                var file = Request.Files[0];
                Manager.Import(Repository, file.InputStream, @override);
                resultEntry.ReloadPage = true;
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);
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
        #endregion


        #region Form

        #region Templates
        public virtual ActionResult Templates(string schemaName)
        {
            ViewData["schema"] = Manager.Get(Repository, schemaName);
            var formTypes = Enum.GetValues(typeof(FormType));

            List<FormModel> formList = new List<FormModel>();

            foreach (var t in formTypes)
            {
                formList.Add(new FormModel
                {
                    Body = Manager.GetForm(Repository, schemaName, (FormType)t),
                    FormType = (FormType)t
                });
            }

            return View(formList);
        }

        [HttpPost]
        public virtual ActionResult Templates(string schemaName, List<FormModel> formList, bool templateBuildByMachine)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                foreach (var form in formList)
                {
                    Manager.SaveForm(Repository, schemaName, form.Body, form.FormType, templateBuildByMachine);
                }
                resultEntry.RedirectUrl = Url.Action("index", new { repositoryName = Repository.Name, siteName = Request.RequestContext.GetRequestValue("siteName") });
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);
            //return View(new FormModel() { FormType = formType, Body = body });
        }

        #endregion

        #region Reset All
        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult ResetAll(string schemaName, FormType CurrentFormType)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                Schema schema = new Schema(Repository, schemaName);

                var formTypes = Enum.GetValues(typeof(FormType));

                List<object> formList = new List<object>();

                foreach (var t in formTypes)
                {
                    formList.Add(new
                    {
                        Body = schema.GenerateForm((FormType)t),
                        FormType = t.ToString()
                    });
                }

                resultEntry.Model = formList;

            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);
        }

        //private void ResetAll(string schemaName)
        //{
        //    ResetAllForm(schemaName);
        //    //this.ResetAll(schemaName, FormType.Grid);
        //}
        private void ResetAllForm(string schemaName)
        {
            Manager.ResetForm(Repository, schemaName, FormType.Create | FormType.Detail | FormType.Grid | FormType.List | FormType.Selectable | FormType.Update);
        }
        #endregion

        #region Reset Current

        [HttpPost]
        [ValidateInput(false)]
        public virtual ActionResult ResetCurrent(FormType CurrentFormType, string schemaName)
        {
            JsonResultEntry resultEntry = new JsonResultEntry();
            try
            {
                var schema = new Schema(Repository, schemaName);
                //Manager.ResetForm(Repository, schemaName, CurrentFormType);

                var form = schema.GenerateForm(CurrentFormType);

                resultEntry.Model = new { Body = form };

                resultEntry.RedirectUrl = Url.Action("Templates", ControllerContext.RequestContext.AllRouteValues().Merge("Repository", Repository.Name)
                    .Merge("schemaName", schemaName).Merge("CurrentFormType", CurrentFormType.ToString()));
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);

            //return RedirectToAction("Templates", new { Repository = Repository.Name, schemaName = schemaName, CurrentFormType = CurrentFormType.ToString() });
        }

        #endregion

        #endregion

        #region Relations
        public virtual ActionResult Relations(string schemaName)
        {
            var model = Manager.GetRelationFolders(new Schema { Name = schemaName, Repository = Repository }).Select(o => new RelationModel()
            {
                RelationName = o.FriendlyName,
                RelationType = "Folder".Localize()
            });
            return View(model);
        }
        #endregion


        public virtual ActionResult ColumnForms(Schema model)
        {
            return View(model);
        }


        /// <summary>
        /// for remote validation
        /// </summary>
        /// <param name="name"></param>
        /// <param name="old_Key"></param>
        /// <returns></returns>
        public virtual ActionResult IsNameAvailable(string name, string old_Key)
        {
            if (old_Key == null || !name.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (Manager.Get(Repository, name) != null)
                {
                    return Json("The name already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        #region Copy

        public virtual ActionResult Copy(string sourceName, string destName)
        {
            var entry = new JsonResultEntry();
            try
            {
                Manager.Copy(Repository, sourceName, destName);
                entry.RedirectUrl = Url.Action("Index", ControllerContext.RequestContext.AllRouteValues());
                entry.ReloadPage = true;
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

        public virtual ActionResult CopyNameAvailabled(string sourceName, string destName)
        {
            return this.IsNameAvailable(destName, null);
        }


        #endregion

    }
}
