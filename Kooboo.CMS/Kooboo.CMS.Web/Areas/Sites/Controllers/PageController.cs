using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.Web;
using Kooboo.Web.Mvc;
using Kooboo.Globalization;

using Kooboo.CMS.Content.Query;
using ContentService = Kooboo.CMS.Content.Services;
using Kooboo.CMS.Content.Models;
using Kooboo.CMS.Sites.DataRule;
using Kooboo.CMS.Web.Models;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Web.Areas.Sites.Models;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Page", Name = "Edit", Order = 1)]
    [ValidateInput(false)]
    public class PageController : PathResourceControllerBase<Page, PageManager>
    {
        Repository Repository
        {
            get
            {
                return GerSiteRepository(Site.Repository);
            }
        }

        public static Repository GerSiteRepository(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return null;
            }
            return new Repository(name).AsActual();
        }

        #region CURD Localize

        public override System.Web.Mvc.ActionResult Index(string search)
        {
            var layoutList = ServiceFactory.LayoutManager.All(Site, "");

            ViewData["LayoutList"] = layoutList;
            return View(List(search));
        }

        public override ActionResult Create()
        {
            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }

            bool isDefault = false;
            bool.TryParse(ControllerContext.RequestContext.GetRequestValue("IsDefault"), out isDefault);
            var page = new Page()
            {
                Layout = ControllerContext.RequestContext.GetRequestValue("layout"),
                IsDefault = isDefault
            };

            return View(page);
        }

        [HttpPost]
        public override ActionResult Create(Page model)
        {

            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                if (ModelState.IsValid)
                {
                    var parentPage = ControllerContext.RequestContext.GetRequestValue("parentPage");
                    Page parent = null;
                    if (!string.IsNullOrWhiteSpace(parentPage))
                    {
                        parent = PageHelper.Parse(Site, parentPage);
                    }

                    model.Parent = parent;

                    SavePosition(model);

                    model.UserName = User.Identity.Name;

                    Manager.Add(Site, parentPage, model);

                    entry.SetSuccess();

                    entry.RedirectUrl = GetReturnUrl();

                }


            }
            catch (Exception e)
            {
                entry.AddException(e);
                entry.SetFailed();
            }

            return Json(entry);

        }


        //public virtual ActionResult CreateTemp(string parentPage, string layout, string returnUrl)
        //{
        //    Page parent = null;
        //    if (!string.IsNullOrWhiteSpace(parentPage))
        //    {
        //        parent = new Page(Site, PageHelper.SplitFullName(parentPage).ToArray());
        //    }
        //    var newPage = Manager.CreateByLayout(Site, parent, layout);

        //    return RedirectToAction("Edit", Request.RequestContext.AllRouteValues().Merge("fullName", newPage.FullName));
        //}
        public override ActionResult Edit(string name)
        {
            #region

            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }

            #endregion
            var o = Get(name);
            return View(o);
        }
        public virtual ActionResult Draft(string name)
        {
            #region

            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }

            #endregion
            var o = Get(name);
            var draft = ServiceFactory.PageManager.PageProvider.GetDraft(o);
            if (draft == null)
            {
                return RedirectToAction("Index", Request.RequestContext.AllRouteValues());
            }
            return View(draft);
        }
        [HttpPost]
        public virtual ActionResult Draft(Page newModel, string old_key, bool? preview, bool? published)
        {
            JsonResultEntry result = new JsonResultEntry();
            try
            {
                newModel.Site = Site;

                var old = Manager.Get(Site, old_key);

                SavePosition(newModel);

                newModel.Published = false;
                newModel.UserName = User.Identity.Name;
                Manager.PageProvider.SaveAsDraft(newModel);

                if (published.HasValue && published.Value == true)
                {
                    Manager.Publish(old, true, User.Identity.Name);

                    result.RedirectUrl = GetReturnUrl();
                }
                result.AddMessage("The item has been saved.".Localize());
            }
            catch (Exception e)
            {
                result.AddException(e);
            }

            return Json(result);
        }

        [HttpPost]
        public override ActionResult Edit(Page newModel, string old_key)
        {
            JsonResultEntry result = new JsonResultEntry();
            try
            {
                newModel.Site = Site;
                newModel.UserName = User.Identity.Name;

                var old = Manager.Get(Site, old_key);

                SavePosition(newModel);

                var saveAsDraft = this.Request.Form["SaveAsDraft"];

                result.RedirectUrl = GetReturnUrl();

                if (!string.IsNullOrEmpty(saveAsDraft) && saveAsDraft.ToLower() == "true")
                {
                    Manager.PageProvider.SaveAsDraft(newModel);
                    result.Model = new { preview = true };
                    //result.RedirectUrl = Url.Action("Draft", ControllerContext.RequestContext.AllRouteValues());
                }
                else
                {
                    var setPublished = Request.Form["SetPublished"];
                    if (!string.IsNullOrEmpty(setPublished))
                    {
                        var published = false;
                        bool.TryParse(setPublished, out published);
                        newModel.Published = published;
                    }
                    else
                    {
                        result.RedirectUrl = null;
                        result.AddMessage("The item has been saved.".Localize());
                    }
                    Manager.Update(Site, newModel, old);
                }

                if (string.IsNullOrEmpty(result.RedirectUrl))
                {
                    result.Model = FrontUrlHelper.Preview(Url, Kooboo.CMS.Sites.Models.Site.Current, newModel, null).ToString();
                }

            }
            catch (Exception e)
            {
                result.AddException(e);
            }

            return Json(result);
        }

        [HttpPost]
        public virtual ActionResult ChangeLayout(string fullName, string layout)
        {
            var entry = new JsonResultEntry();

            try
            {
                var old = Manager.Get(Site, fullName);
                old.Layout = layout;
                old.UserName = User.Identity.Name;
                Manager.Update(Site, old, old);
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

        private void SavePosition(Page newModel)
        {
            var json = Request.Form["PagePositionsJson"];
            if (!string.IsNullOrEmpty(json))
            {
                var positions = PageDesignController.ParsePagePositions(json);
                newModel.PagePositions = positions;
            }
        }

        protected override ActionResult RedirectToIndex()
        {
            string fullName = Request["fullName"];
            return RedirectToIndex(fullName);
        }
        protected override ActionResult RedirectToIndex(string newFullName)
        {
            var routes = this.ControllerContext.RequestContext.AllRouteValues();
            if (!string.IsNullOrEmpty(newFullName))
            {
                routes["fullName"] = newFullName;
            }
            return RedirectToAction("Index", routes);
        }
        protected override Page Get(string name)
        {
            string fullName = Request["fullName"];
            return Manager.Get(Site, fullName);
        }
        protected override void Update(Page @new, string oldFullName)
        {
            throw new NotSupportedException("The implement is in Edit action method.");
            //var old = Manager.Get(Site, oldFullName);
            //@new = Manager.Get(Site, oldFullName);
            //if (TryUpdateModel(@new))
            //{
            //    Manager.Update(Site, @new, old);
            //}
        }
        protected override void Remove(Page model)
        {
            model.Site = Site;
            Manager.Remove(Site, model);
        }
        protected override IEnumerable<Page> List(string search)
        {
            string fullName = Request["fullName"];

            if (fullName != null)
            {
                ViewData["page"] = Manager.Get(Site, fullName);
            }


            var list = Manager.All(Site, fullName, search);

            return list;
        }

        public virtual ActionResult Localize(string fullName)
        {
            Manager.Localize(fullName, Site);

            var fullNameArray = PageHelper.SplitFullName(fullName);

            if (!string.IsNullOrWhiteSpace(this.Request.RequestContext.GetRequestValue("ReturnUrl")))
            {
                return Redirect(this.Request.RequestContext.GetRequestValue("ReturnUrl"));
            }
            return RedirectToAction("Index", new { fullName = PageHelper.CombineFullName(fullNameArray.Take(fullNameArray.Count() - 1)) });


        }

        public override ActionResult Unlocalize(Page o)
        {
            var entry = new JsonResultEntry();
            try
            {
                o.Site = Site;
                Manager.Unlocalize(o);
                entry.RedirectUrl = GetReturnUrl();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);


        }
        #endregion

        #region import/export
        public void Export(Page[] model)
        {
            //var fullNameArray = model.Select(o => o.FullName);
            //var selected = Manager.All(Site, "").Where(o => fullNameArray.Contains(o.FullName));

            var fileName = GetZipFileName();
            Response.AttachmentHeader(fileName);
            foreach (var item in model)
            {
                item.Site = Site;
            }
            Manager.Export(model, Response.OutputStream);
        }

        protected string GetZipFileName()
        {
            var fullName = Request.RequestContext.GetRequestValue("FullName");

            if (string.IsNullOrWhiteSpace(fullName))
            {
                return "Pages.zip";
            }

            return "Page." + fullName + ".zip";
        }



        public virtual ActionResult Import(string fullName, bool @override)
        {
            JsonResultEntry resultEntry = new JsonResultEntry(ModelState);
            try
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    Manager.Import(Site, fullName, Request.Files[0].InputStream, @override);
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
                ContentList = folder.CreateQuery(),
                FolderList = subfolders,
                CategoryFolders = folder.Categories == null ? (new List<TextFolder>()) : folder.Categories.Select(o => FolderHelper.Parse<TextFolder>(Repository, o.FolderName))
            };

            return Json(folderInfo);
        }


        public virtual ActionResult DataRuleGridForms(DataRuleSetting[] DataRules)
        {
            return View(DataRules);
        }
        #endregion

        #region IsNameAvailable
        public virtual ActionResult IsNameAvailable(string name, string parentPage, string old_Key)
        {
            if (old_Key == null || !name.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                string fullName = PageHelper.CombineFullName(new[]
                {
                    parentPage,
                    name
                });
                var page = new Page(Site, fullName);
                if (page.Exists() && page.IsLocalized(Site))
                {
                    return Json("The name already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public virtual ActionResult IsIdentifierAvailable(string fullName, string parentPage, string name, PageRoute route)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (!string.IsNullOrEmpty(parentPage))
                    {
                        fullName = fullName + PageHelper.NameSplitter + name;
                    }
                    else
                    {
                        fullName = name;
                    }
                }
            }

            if (!string.IsNullOrEmpty(fullName) && route != null && !string.IsNullOrEmpty(route.Identifier))
            {
                var page = new Page(Site.Current, fullName);
                page.Route = route;
                if (!route.Identifier.StartsWith("#"))
                {
                    if (route.Identifier == "*")
                    {
                        if (page.Parent == null)
                        {
                            return Json("The alternative name of '*' can not be set at root page.".Localize(), JsonRequestBehavior.AllowGet);
                        }
                    }
                    else
                    {
                        var foundPage = Manager.GetPageByUrlIdentifier(Site, page.VirtualPath);
                        if (foundPage != null)
                        {
                            if (foundPage != page)
                            {
                                return Json("The alternative name already exists.".Localize(), JsonRequestBehavior.AllowGet);
                            }
                        }
                    }
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region COPY PAGE
        public virtual ActionResult CopyPage()
        {
            return View();
        }

        [HttpPost]
        public virtual ActionResult CopyPage(string sourcePage, string parentPage, string name)
        {
            JsonResultEntry entry = new JsonResultEntry();

            var destPage = PageHelper.CombineFullName(new string[] { parentPage, name });
            try
            {
                Manager.Copy(Site, sourcePage, destPage);
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }
        #endregion

        #region Move Page
        public virtual ActionResult MovePage(string fullName)
        {
            CopyPageModel model = new CopyPageModel();
            model.SourcePage = new Page(Site, fullName);
            return View(model);
        }

        [HttpPost]
        public virtual ActionResult MovePage(CopyPageModel model, string fullName, string parentPage)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                Manager.Move(Site, fullName, parentPage, model.CreateRedirect);
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }

        /// <summary>
        /// Sorts the pages.
        /// </summary>
        /// <param name="fullName">The full name. The parent page name</param>
        /// <param name="pageNames">The page names order</param>
        /// <returns></returns>
        public virtual ActionResult SortPages(string fullName, string[] pageNames)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                Manager.SortPages(Site, fullName, pageNames);
                entry.SetSuccess();
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }
            return Json(entry);
        }
        #endregion

        #region Page Selector
        public virtual ActionResult PageSelector()
        {
            return View();
        }
        #endregion

        #region Publish/Unpublish
        public virtual ActionResult Publish(string fullName)
        {
            PagePublishViewModel model = new PagePublishViewModel() { FullName = fullName, PublishDate = DateTime.Now, OfflineDate = DateTime.Now, PublishTime = "00:00", OfflineTime = "00:00" };
            return View(model);
        }
        [HttpPost]
        public virtual ActionResult Publish(PagePublishViewModel model)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                if (ModelState.IsValid)
                {
                    var page = new Page(Site, model.FullName);
                    var publishDate = DateTime.Parse(model.PublishDate.ToShortDateString() + " " + model.PublishTime);
                    var offlineDate = DateTime.Parse(model.OfflineDate.ToShortDateString() + " " + model.OfflineTime);
                    ServiceFactory.PageManager.Publish(page, model.PublishSchedule, model.PublishDraft, model.Period, publishDate, offlineDate, User.Identity.Name);
                }
                entry.RedirectToOpener = true;
                entry.RedirectUrl = Url.Action("Index", this.ControllerContext.RequestContext.AllRouteValues());
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }
        [HttpPost]
        public virtual ActionResult Unpublish(string fullName)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                if (ModelState.IsValid)
                {
                    var page = new Page(Site, fullName);
                    ServiceFactory.PageManager.Unpublish(page, User.Identity.Name);
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }
        [HttpPost]
        public virtual ActionResult BatchPublish(Page[] model)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                foreach (var page in model)
                {
                    page.Site = Site;
                    ServiceFactory.PageManager.Publish(page, false, User.Identity.Name);
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }
        [HttpPost]
        public virtual ActionResult BatchUnpublish(Page[] model)
        {
            JsonResultEntry entry = new JsonResultEntry();
            try
            {
                foreach (var page in model)
                {
                    page.Site = Site;
                    ServiceFactory.PageManager.Unpublish(page, User.Identity.Name);
                }
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }
        #endregion

        #region Version
        public virtual ActionResult Version(string fullName)
        {
            var page = new Page(Site, fullName);
            var model = Manager.VersiongLogger.AllVersions(page);
            return View(model);
        }

        public virtual ActionResult Revert(string fullName, int version)
        {
            var entry = new JsonResultEntry();
            var page = new Page(Site, fullName);

            try
            {
                Manager.VersiongLogger.Revert(page, version);
                entry.SetSuccess().AddMessage("Revert Successfully.");
            }
            catch (Exception e)
            {
                entry.AddException(e);
            }

            return Json(entry);
        }

        public virtual ActionResult PreviewVersion(string fullName, int version)
        {

            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }
            var page = new Page(Site, fullName);
            var model = Manager.VersiongLogger.GetVersion(page, version);
            return View(model);
        }
        #endregion
    }
}
