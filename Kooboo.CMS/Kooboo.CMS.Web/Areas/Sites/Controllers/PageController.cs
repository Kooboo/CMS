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
using Kooboo.CMS.Sites.Persistence;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Web;
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
using ContentService = Kooboo.CMS.Content.Services;
namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [ValidateInput(false)]
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Page", Name = "Edit", Order = 1)]
    public class PageController : PathResourceControllerBase<Page, PageManager>
    {
        #region .ctor
        IPageProvider PageProvider { get; set; }
        PageCachingManager PageCachingManager { get; set; }
        public PageController(IPageProvider pageProvider, PageManager manager, PageCachingManager pageCachingManager)
            : base(manager)
        {
            this.PageProvider = pageProvider;
            this.PageCachingManager = pageCachingManager;
        }
        #endregion

        #region Index
        public override System.Web.Mvc.ActionResult Index(string search, string sortField, string sortDir, int? page, int? pageSize)
        {
            var layoutList = ServiceFactory.LayoutManager.All(Site, "");

            ViewData["LayoutList"] = layoutList;
            return View(List(search, sortField, sortDir));
        }
        protected override IEnumerable<Page> List(string search, string sortField, string sortDir)
        {
            string parentPage = this.ControllerContext.RequestContext.GetRequestValue("parentPage");

            if (parentPage != null)
            {
                ViewData["page"] = Manager.Get(Site, parentPage);
            }


            var list = Manager.All(Site, parentPage, search);

            return list.AsQueryable().SortBy(sortField, sortDir);
        }

        #endregion

        #region Create
        public override ActionResult Create()
        {
            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }
            var parentPage = ControllerContext.RequestContext.GetRequestValue("parentPage");
            Page parent = null;
            if (!string.IsNullOrEmpty(parentPage))
            {
                parent = new Page(parentPage);
            }
            bool isDefault = false;
            bool.TryParse(ControllerContext.RequestContext.GetRequestValue("IsDefault"), out isDefault);
            var page = Page.Activator();

            page.Parent = parent;
            page.Site = Site;
            page.Layout = ControllerContext.RequestContext.GetRequestValue("layout");
            page.IsDefault = isDefault;

            return View(page);
        }

        [HttpPost]
        public virtual ActionResult CreateEx(Page model, bool? setPublished, string @return, string PageOrders)
        {
            var data = new JsonResultData() { RedirectUrl = @return };

            if (ModelState.IsValid)
            {
                data.RunWithTry((resultData) =>
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

                    if (setPublished.HasValue && setPublished.Value == true)
                    {
                        model.Published = true;
                    }
                    else
                    {
                        model.Published = false;
                    }
                    Manager.Add(Site, parentPage, model);

                    if (!string.IsNullOrEmpty(PageOrders))
                    {
                        var pageOrders = PageOrders.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        for (int i = 0; i < pageOrders.Length - 1; i++)
                        {
                            if (pageOrders[i] == "!NEWPAGE!")
                            {
                                pageOrders[i] = model.FullName;
                            }
                        }
                        Manager.SortPages(Site, parent == null ? "" : parent.FullName, pageOrders);
                    }

                    resultData.RedirectUrl = @return;
                });
            }

            return Json(data);

        }
        #endregion

        #region Edit
        protected override Page Get(string fullName)
        {
            return Manager.Get(Site, fullName);
        }
        protected override void Update(Page @new, string oldFullName)
        {
            throw new NotSupportedException("The implementation is in Edit action method.");
        }
        public override ActionResult Edit(string uuid)
        {
            #region

            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }

            #endregion
            var o = Get(uuid);
            return View(o);
        }
        [HttpPost]
        public virtual ActionResult Save(Page newModel, string old_key, bool? saveAsDraft, bool? setPublished, string @return, string PageOrders)
        {
            var data = new JsonResultData();
            data.RunWithTry((resultData) =>
                {
                    newModel.Site = Site;
                    newModel.UserName = User.Identity.Name;

                    var old = Manager.Get(Site, old_key);

                    SavePosition(newModel);

                    if (saveAsDraft.HasValue && saveAsDraft.Value == true)
                    {
                        PageProvider.SaveAsDraft(newModel);
                        resultData.Model = new { preview = true };
                        resultData.RedirectUrl = Url.Action("Draft", ControllerContext.RequestContext.AllRouteValues());
                    }
                    else
                    {
                        if (setPublished.HasValue)
                        {
                            newModel.Published = setPublished.Value;
                            data.RedirectUrl = @return;
                        }
                        else
                        {
                            resultData.AddMessage("The item has been saved.".Localize());
                        }
                        Manager.Update(Site, newModel, old);

                    }

                    if (!string.IsNullOrEmpty(PageOrders))
                    {
                        var pageOrders = PageOrders.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
                        Manager.SortPages(Site, old.Parent == null ? "" : old.Parent.FullName, pageOrders);
                    }

                });
            return Json(data);
        }
        [HttpPost]
        public virtual ActionResult ChangeLayout(string uuid, string layout)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                var old = Manager.Get(Site, uuid);
                old.Layout = layout;
                old.UserName = User.Identity.Name;
                Manager.Update(Site, old, old);

                resultData.ReloadPage = true;
            });
            return Json(data);
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
        #endregion

        #region Draft
        public virtual ActionResult Draft(string uuid)
        {
            #region

            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }

            #endregion
            var o = Get(uuid);
            var draft = PageProvider.GetDraft(o);
            if (draft == null)
            {
                return RedirectToAction("Index", Request.RequestContext.AllRouteValues());
            }
            return View(draft);
        }
        [HttpPost]
        public virtual ActionResult Draft(Page newModel, string old_key, bool? preview, bool? setPublished, string @return)
        {
            var data = new JsonResultData();
            data.RunWithTry((resultData) =>
                {
                    newModel.Site = Site;

                    var old = Manager.Get(Site, old_key);

                    SavePosition(newModel);

                    newModel.Published = false;
                    newModel.UserName = User.Identity.Name;
                    PageProvider.SaveAsDraft(newModel);

                    if (setPublished.HasValue && setPublished.Value == true)
                    {
                        Manager.Publish(old, true, User.Identity.Name);

                        data.RedirectUrl = @return;
                    }
                    else
                    {
                        resultData.AddMessage("The item has been saved.".Localize());
                    }

                });
            return Json(data);
        }

        #endregion

        #region Delete
        [HttpPost]
        public virtual ActionResult Delete1(Page model, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {

                Remove(model);

                data.RedirectUrl = @return;
            });

            return Json(data);

        }
        protected override void Remove(Page model)
        {
            model.Site = Site;
            Manager.Remove(Site, model);
        }
        #endregion

        #region Localize
        public virtual ActionResult Localize(Page[] model)
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
        public virtual ActionResult Localize1(string uuid)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                Manager.Localize(uuid, Site);
                data.ReloadPage = true;
            });
            return Json(data);
        }
        #endregion

        #region Unlocalize
        public override ActionResult Unlocalize(Page o, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                o.Site = Site;
                Manager.Unlocalize(o);
                resultData.RedirectUrl = @return;
            });
            return Json(data);
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
            var fullName = Request.RequestContext.GetRequestValue("UUID");

            if (string.IsNullOrWhiteSpace(fullName))
            {
                return "Pages.zip";
            }

            return "Page." + fullName + ".zip";
        }

        public virtual ActionResult Import()
        {
            return View();
        }
        [HttpPost]
        public virtual ActionResult Import(string uuid, bool @override, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (Request.Files.Count > 0 && Request.Files[0].ContentLength > 0)
                {
                    Manager.Import(Site, uuid, Request.Files[0].InputStream, @override);
                }
                data.RedirectUrl = @return;
            });
            return Json(data, "text/plain", System.Text.Encoding.UTF8);
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
                ContentList = folder.CreateQuery(),
                FolderList = subfolders,
                CategoryFolders = folder.Categories == null ? (new List<TextFolder>()) : folder.Categories.Select(o => FolderHelper.Parse<TextFolder>(Repository.Current, o.FolderName))
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
            if (AreaRegistrationEx.AllAreas.Contains(name, StringComparer.OrdinalIgnoreCase))
            {
                return Json("The name is unavailable for page, it is already used as a MVC area name.", JsonRequestBehavior.AllowGet);
            }

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
        public virtual ActionResult IsIdentifierAvailable(string uuid, string parentPage, string name, PageRoute route)
        {
            if (string.IsNullOrEmpty(uuid))
            {
                if (!string.IsNullOrEmpty(name))
                {
                    if (!string.IsNullOrEmpty(parentPage))
                    {
                        uuid = uuid + PageHelper.NameSplitter + name;
                    }
                    else
                    {
                        uuid = name;
                    }
                }
            }

            if (!string.IsNullOrEmpty(uuid) && route != null && !string.IsNullOrEmpty(route.Identifier))
            {
                var page = new Page(Site.Current, uuid);
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

        public virtual ActionResult CopyNameAvailabled(CopyPageModel copyModel)
        {
            return this.IsNameAvailable(copyModel.DestinationName, copyModel.ParentPage, null);
        }
        #endregion

        #region Copy
        public virtual ActionResult Copy(string uuid)
        {
            return View(new CopyPageModel() { UUID = uuid });
        }

        [HttpPost]
        public virtual ActionResult Copy(CopyPageModel copyModel, string @return)
        {
            var data = new JsonResultData(ModelState);

            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    var destPage = PageHelper.CombineFullName(new string[] { copyModel.ParentPage, copyModel.DestinationName });
                    Manager.Copy(Site, copyModel.UUID, destPage);
                    data.RedirectUrl = @return;
                }
            });

            return Json(data);
        }
        #endregion

        #region Move
        public virtual ActionResult Move(string uuid)
        {
            return View(new MovePageModel() { UUID = uuid });
        }

        [HttpPost]
        public virtual ActionResult Move(MovePageModel model, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    Manager.Move(Site, model.UUID, model.ParentPage, model.CreateRedirect);
                    data.RedirectUrl = @return;
                }
            });
            return Json(data);
        }

        /// <summary>
        /// Sorts the pages.
        /// </summary>
        /// <param name="fullName">The full name. The parent page name</param>
        /// <param name="pageNames">The page names order</param>
        /// <returns></returns>
        public virtual ActionResult SortPages(string uuid, string[] pageNames)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                Manager.SortPages(Site, uuid, pageNames);
            });
            return Json(data);
        }
        #endregion

        #region Publish/Unpublish
        public virtual ActionResult Publish(string uuid)
        {
            PagePublishViewModel model = new PagePublishViewModel() { UUID = uuid, PublishDate = DateTime.Now, OfflineDate = DateTime.Now, PublishTime = "00:00", OfflineTime = "00:00" };
            return View(model);
        }
        [HttpPost]
        public virtual ActionResult Publish(PagePublishViewModel model, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    var page = new Page(Site, model.UUID);
                    var publishDate = DateTime.Parse(model.PublishDate.ToShortDateString() + " " + model.PublishTime);
                    var offlineDate = DateTime.Parse(model.OfflineDate.ToShortDateString() + " " + model.OfflineTime);
                    ServiceFactory.PageManager.Publish(page, model.PublishSchedule, model.PublishDraft, model.Period, publishDate, offlineDate, User.Identity.Name);
                }
                resultData.ReloadPage = true;
                resultData.RedirectUrl = @return;
            });
            return Json(data);
        }
        [HttpPost]
        public virtual ActionResult Unpublish(string uuid, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                if (ModelState.IsValid)
                {
                    var page = new Page(Site, uuid);
                    ServiceFactory.PageManager.Unpublish(page, User.Identity.Name);
                    resultData.ReloadPage = true;
                    resultData.RedirectUrl = @return;
                }
            });

            return Json(data);
        }
        [HttpPost]
        public virtual ActionResult BatchPublish(Page[] model)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                foreach (var page in model)
                {
                    page.Site = Site;
                    ServiceFactory.PageManager.Publish(page, false, User.Identity.Name);
                }
                data.ReloadPage = true;
            });

            return Json(data);
        }
        [HttpPost]
        public virtual ActionResult BatchUnpublish(Page[] model)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                foreach (var page in model)
                {
                    page.Site = Site;
                    ServiceFactory.PageManager.Unpublish(page, User.Identity.Name);
                }
                data.ReloadPage = true;
            });

            return Json(data);
        }
        #endregion

        #region Versions
        public virtual ActionResult Versions(string uuid)
        {
            var page = new Page(Site, uuid);
            var model = Manager.VersiongLogger.AllVersions(page);
            return View(model);
        }

        public virtual ActionResult PreviewVersion(string uuid, int version)
        {

            if (!string.IsNullOrWhiteSpace(Site.Repository))
            {
                GenerateFolderListDicViewData();
            }
            var page = new Page(Site, uuid);
            var model = Manager.VersiongLogger.GetVersion(page, version);
            return View(model);
        }

        public virtual ActionResult Revert(string uuid, int version, string @return)
        {
            var data = new JsonResultData(ModelState);
            data.RunWithTry((resultData) =>
            {
                var page = new Page(Site, uuid);
                Manager.VersiongLogger.Revert(page, version);
                resultData.RedirectUrl = @return;
            });

            return Json(data);
        }

        #endregion

        #region DeleteDiskCachings
        public ActionResult DeleteDiskCachings(string uuid)
        {
            var data = new JsonResultData();
            data.RunWithTry((resultData) =>
            {
                PageCachingManager.DeleteCaching(new Page(Site, uuid));
                resultData.AddMessage("The disk caching has been deleted.");
            });
            return Json(data);
        }
        #endregion
    }
}
