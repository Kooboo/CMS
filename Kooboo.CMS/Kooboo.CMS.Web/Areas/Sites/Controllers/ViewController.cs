#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Common;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Content.Query;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.Versioning;
using Kooboo.CMS.Web.Areas.Sites.Models;
using Kooboo.CMS.Web.Models;
using Kooboo.Globalization;
using Kooboo.Web;
using Kooboo.Web.Mvc;
using Kooboo.Web.Mvc.Paging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using ContentService = Kooboo.CMS.Content.Services;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "View", Order = 1)]
    public class ViewController : PathResourceControllerBase<Kooboo.CMS.Sites.Models.View, ViewManager>
    {
        #region .ctor
        public ViewController(ViewManager manager) : base(manager) { }
        #endregion

        #region Index
        protected override IEnumerable<CMS.Sites.Models.View> List(string search, string sortField, string sortDir)
        {
            string ns = ControllerContext.RequestContext.GetRequestValue("ns");
            return ((ViewManager)Manager).ByNamespace(Site, ns, search);
        }

        public override System.Web.Mvc.ActionResult Index(string search, string sortField, string sortDir, int? page, int? pageSize)
        {
            string nsStr = ControllerContext.RequestContext.GetRequestValue("ns");

            var ns = Manager.GetNamespace(Site).GetNamespaceNode(nsStr);
            ViewData["NameSpace"] = ns;
            return View(List(search, sortField, sortDir));
        }

        #endregion

        #region Localize
        public virtual ActionResult Localize(View[] model)
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

        #region Create
        public override ActionResult Create(View model)
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

            model.Site = Site;
            model.Body = body;
            model.Name = name;

            return base.Create(model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public override ActionResult Create(CMS.Sites.Models.View model, string @return)
        {
            var data = new JsonResultData(ModelState) { RedirectUrl = @return };
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
        }
        #endregion

        #region Edit
        public override ActionResult Edit(string uuid)
        {
            var model = Get(uuid);
            ViewData["ViewEngine"] = Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(model.EngineName);
            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }


            return View(model);
        }


        [HttpPost]
        [ValidateInput(false)]
        public override ActionResult Edit(CMS.Sites.Models.View newModel, string old_key, string @return)
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

        protected override void Update(View model, string old_key)
        {
            var oldModel = Manager.Get(Site, old_key);
            model.Site = Site;
            Manager.Update(Site, model, oldModel);
        }
        #endregion

        #region DeleteViewNamespace
        public virtual ActionResult DeleteViewNamespace(View[] model, string[] selectedNamespace)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                Manager.All(Site, null).Where(o => o.Name.StartsWith(selectedNamespace.First()));
            });
            return Json(data);
        }
        #endregion

        #region Relations
        public virtual ActionResult Relations(string uuid)
        {
            var model = Manager.Relations(new CMS.Sites.Models.View { Name = uuid, Site = Site });
            return View("Relations", model);
        }
        #endregion

        #region IsNameAvailable
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
        #endregion

        #region DataRule Settings
        public virtual ActionResult DataRuleFolderGrid()
        {
            return View();
        }

        private void GenerateFolderListDicViewData()
        {
            var folderTree = ContentService.ServiceFactory.TextFolderManager.FolderTrees(Repository.Current);

            ViewData["FolderTree"] = folderTree;
        }



        public virtual ActionResult GetFolderInfo(string folderPath)
        {
            if (string.IsNullOrWhiteSpace(folderPath))
            {
                return null;
            }

            var folder = (new TextFolder(Repository.Current, FolderHelper.SplitFullName(folderPath))).AsActual();

            if (folder == null)
            {
                return null;
            }

            var subfolders = ContentService.ServiceFactory.TextFolderManager.ChildFolders(folder);

            var schema = ContentService.ServiceFactory.SchemaManager.Get(Repository.Current, folder.SchemaName);

            var folderInfo = new
            {
                Folder = folder,
                Schema = schema,
                //ContentList = folder.CreateQuery(),
                FolderList = subfolders,
                CategoryFolders = folder.Categories == null ? (new List<TextFolder>()) : folder.Categories.Select(o => FolderHelper.Parse<TextFolder>(Repository.Current, o.FolderName))
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

        #region Versions
        public virtual ActionResult Versions(View view)
        {
            view.Site = Site;
            var model = VersionManager.AllVersions<View>(view);
            return View(model);
        }
        public virtual ActionResult Revert(View view, int version, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                view.Site = Site;
                VersionManager.Revert<View>(view, version);
                resultData.RedirectUrl = @return;
            });

            return Json(data);
        }
        public virtual ActionResult PreviewVersion(View view, int version)
        {
            view.Site = Site;
            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }
            var model = VersionManager.GetVersion<View>(view, version);
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
        public virtual void Export(Kooboo.CMS.Sites.Models.View[] model)
        {
            var fileName = "Views.zip";
            Response.AttachmentHeader(fileName);
            if (model != null)
            {
                foreach (var item in model)
                {
                    item.Site = Site;
                }
            }
            Manager.Provider.Export(Site, model, Response.OutputStream);
        }
        #endregion
        #endregion

        #region GetCodeSnippet
        public virtual ActionResult GetCodeSnippet(DataRuleSetting dataRuleSetting)
        {
            var viewEngineName = Request.QueryString["ViewEngine"];
            var viewEngine = Kooboo.CMS.Sites.View.TemplateEngines.GetEngineByName(viewEngineName);
            var codeSnippetClean = viewEngine.GetDataRuleCodeSnippet(dataRuleSetting.TakeOperation).Generate(Site.Current.GetRepository(), dataRuleSetting, false);
            var codeSnippetInline = viewEngine.GetDataRuleCodeSnippet(dataRuleSetting.TakeOperation).Generate(Site.Current.GetRepository(), dataRuleSetting, true);
            var data = new
                {
                    DataRuleName = dataRuleSetting.DataName,
                    CodeSnippetClean = codeSnippetClean,
                    CodeSnippetInline = codeSnippetInline
                };
            return Json(data);
        }
        #endregion
    }
}
