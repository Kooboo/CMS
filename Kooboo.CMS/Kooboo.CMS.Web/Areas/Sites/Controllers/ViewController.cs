using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.Web.Mvc;
using Kooboo.Globalization;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Web.Models;
using ContentService = Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites;
using Kooboo.Web;
using Kooboo.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "View", Order = 1)]
    public class ViewController : PathResourceControllerBase<Kooboo.CMS.Sites.Models.View, ViewManager>
    {
        Repository Repository
        {
            get
            {
                if (string.IsNullOrEmpty(Site.Repository))
                {
                    return null;
                }

                return new Repository(Site.Repository).AsActual();
            }
        }

        protected override IEnumerable<CMS.Sites.Models.View> List(string search)
        {
            string ns = ControllerContext.RequestContext.GetRequestValue("ns");
            return ((ViewManager)Manager).ByNamespace(Site, ns, search);
        }

        public override System.Web.Mvc.ActionResult Index(string search)
        {
            string nsStr = ControllerContext.RequestContext.GetRequestValue("ns");

            var ns = Manager.GetNamespace(Site).GetNamespaceNode(nsStr);
            ViewData["NameSpace"] = ns;
            return View(List(search));
        }

        public virtual ActionResult Localize(string name)
        {
            Manager.Localize(name, Site);
            return RedirectToIndex();
        }
        public override ActionResult Create()
        {
            var engineName = ControllerContext.RequestContext.GetRequestValue("EngineName");
            var engine = Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(engineName);
            ViewData["ViewEngine"] = engine;
            if (!string.IsNullOrEmpty(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }

            string name = Request.RequestContext.GetRequestValue("ns");
            name = !string.IsNullOrEmpty(name) ? name + "." : null;

            string body = engine.GetCodeHelper().DefaultViewCode();
            body = string.IsNullOrWhiteSpace(body) ? " " : body;
            var view = new View() { Site = Site, EngineName = engineName, Body = body, Name = name };


            return View(view);
        }
        [HttpPost]
        [ValidateInput(false)]
        public override ActionResult Create(CMS.Sites.Models.View model)
        {
            JsonResultEntry resultEntry = new JsonResultEntry() { Success = true };
            try
            {
                if (ModelState.IsValid)
                {
                    model.UserName = User.Identity.Name;
                    Add(model);
                    var pageRedirect = Request.Form["pageRedirect"] ?? "";
                    resultEntry.SetSuccess().RedirectUrl = pageRedirect.ToLower() == "false" ?
                        Url.Action("Edit", ControllerContext.RequestContext.AllRouteValues().Merge("Name", model.Name))
                        : Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues());
                }
                else
                {
                    resultEntry.AddModelState(ModelState).SetFailed();
                }
            }
            catch (Exception e)
            {
                resultEntry.SetFailed().AddException(e);
            }
            resultEntry.AddModelState(ViewData.ModelState);
            return Json(resultEntry);
        }
        public override ActionResult Edit(string name)
        {
            var model = Get(name);
            ViewData["ViewEngine"] = Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(model.EngineName);
            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }


            return View(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        public override ActionResult Edit(CMS.Sites.Models.View newModel, string old_key)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                if (ModelState.IsValid)
                {
                    newModel.UserName = User.Identity.Name;
                    Update(newModel, old_key);
                    var pageRedirect = Request.Form["pageRedirect"];
                    entry.AddMessage("The item has been saved.".Localize()).SetSuccess().RedirectUrl = string.Equals(pageRedirect, "true", StringComparison.OrdinalIgnoreCase) ? this.Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues()) : null;

                }
                else
                {
                    entry.AddModelState(ModelState);
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

        protected override void Update(View model, string old_key)
        {
            var oldModel = Manager.Get(Site, old_key);
            model.Site = Site;
            Manager.Update(Site, model, oldModel);
        }

        public virtual ActionResult DeleteViewNamespace(View[] model, string[] selectedNamespace)
        {
            JsonResultEntry entry = new JsonResultEntry();
            Manager.All(Site, null).Where(o => o.Name.StartsWith(selectedNamespace.First()));
            return Json(entry);
        }

        public virtual ActionResult Relations(string name)
        {
            var model = Manager.RelationsPages(new CMS.Sites.Models.View { Name = name, Site = Site }).Select(o => new RelationModel
            {
                RelationName = o.FriendlyName,
                RelationType = "Page".Localize()
            });
            return View("Relations", model);
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
                if (Manager.Get(Site, name) != null)
                {
                    return Json("The name already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        #region DataRule Settings
        public virtual ActionResult DataRuleFolderGrid()
        {
            return View();
        }

        private void GenerateFolderListDicViewData()
        {
            var folderTree = ContentService.ServiceFactory.TextFolderManager.FolderTrees(Repository);

            ViewData["FolderTree"] = folderTree;
        }



        public virtual ActionResult GetFolderInfo(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                return null;
            }

            var folder = Kooboo.CMS.Content.Models.IPersistableExtensions.AsActual(new TextFolder(Repository, FolderHelper.SplitFullName(folderPath)));

            if (folder == null)
            {
                return null;
            }

            var subfolders = ContentService.ServiceFactory.TextFolderManager.ChildFolders(folder);

            var schema = ContentService.ServiceFactory.SchemaManager.Get(Repository, folder.SchemaName);

            var folderInfo = new
            {
                Folder = folder,
                Schema = schema,
                //ContentList = folder.CreateQuery(),
                FolderList = subfolders,
                CategoryFolders = folder.Categories == null ? (new List<TextFolder>()) : folder.Categories.Select(o => FolderHelper.Parse<TextFolder>(Repository, o.FolderName))
            };

            return Json(folderInfo);
        }


        public virtual ActionResult DataRuleGridForms(DataRuleSetting[] DataRules, string engine)
        {
            ViewData["ViewEngine"] = Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(engine);
            return View(DataRules);
        }

        #endregion

        #region Copy

        public virtual ActionResult Copy(string sourceName, string destName)
        {
            var entry = new JsonResultEntry();

            try
            {
                Manager.Copy(Site, sourceName, destName);
                entry.RedirectUrl = this.GetReturnUrl();
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

        #region Versions
        public virtual ActionResult Version(View view)
        {
            view.Site = Site;
            var model = Manager.VersiongLogger.AllVersions(view);
            return View(model);
        }
        public virtual ActionResult Revert(View view, int version)
        {
            var entry = new JsonResultEntry();

            try
            {
                view.Site = Site;
                Manager.VersiongLogger.Revert(view, version);
                entry.AddMessage("Revert successfully.".Localize());
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }
        public virtual ActionResult PreviewVersion(View view, int version)
        {
            view.Site = Site;
            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }
            var model = Manager.VersiongLogger.GetVersion(view, version);
            ViewData["ViewEngine"] = Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(model.EngineName);
            return View(model);
        }
        #endregion

        #region Import/Export
        #region Import
        public virtual ActionResult Import()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult Import(bool @override)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    Manager.Provider.Import(Site, Request.Files[0].InputStream, @override);
                }
                resultEntry.ReloadPage = true;
            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry, "text/plain", System.Text.Encoding.UTF8);
        }
        #endregion

        #region Export
        [HttpPost]
        public virtual void Export(Kooboo.CMS.Sites.Models.View[] model)
        {
            var fileName = "Views.zip";
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
