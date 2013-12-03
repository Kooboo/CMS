using Kooboo.CMS.Common;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Models;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.CMS.Modules.Publishing.Cmis;
using Kooboo.CMS.Modules.Publishing.Services;
using Kooboo.CMS.Modules.Publishing.Models;

namespace Kooboo.CMS.Modules.Publishing.Web.Areas.Publishing.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Publishing", Group = "Remote", Name = "TextFolderMapping", Order = 1)]
    public class RemoteTextFolderMappingController : AreaControllerBase
    {
        #region .ctor
        private readonly RemoteTextFolderMappingManager _manager;
        private readonly RemoteSettingManager _remoteEndPointSettingManager;
        private readonly ICmisSession _cmisSession;
        public RemoteTextFolderMappingController(ICmisSession cmisSession, RemoteTextFolderMappingManager manager, RemoteSettingManager remoteEndPointSettingManager)
        {
            this._cmisSession = cmisSession;
            this._manager = manager;
            this._remoteEndPointSettingManager = remoteEndPointSettingManager;
        }
        #endregion

        #region Index
        public ActionResult Index(string siteName, string search, string sortField, string sortDir)
        {
            var query = this._manager.CreateQuery(siteName);
            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(it => it.Name.Contains(search));
            }
            if (!string.IsNullOrWhiteSpace(sortField))
            {
                query = query.SortByField(sortField, sortDir);
            }
            return View(query.ToList());
        }
        #endregion

        #region Create
        public ActionResult Create()
        {
            var model = new RemoteTextFolderMapping();
            model.Enabled = true;
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RemoteTextFolderMapping mapping, string @return)
        {
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                resultEntry.RunWithTry((data) =>
                {
                    _manager.Add(mapping);
                    data.RedirectUrl = @return;
                });
            }
            return Json(resultEntry);
        }
        #endregion

        #region Edit
        public ActionResult Edit(string uuid)
        {
            var model = _manager.Get(uuid);
            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(RemoteTextFolderMapping mapping, string @return)
        {
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                resultEntry.RunWithTry((data) =>
                {
                    var oldModel = _manager.Get(mapping.UUID);
                    _manager.Update(mapping, oldModel);
                    resultEntry.RedirectUrl = @return;
                });
            }
            return Json(resultEntry);
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete(DeleteModel[] model)
        {
            var resultEntry = new JsonResultData(ModelState);
            if (ModelState.IsValid)
            {
                resultEntry.RunWithTry((data) =>
                {
                    var uuids = model.Select(it => it.UUID).ToArray();
                    if (uuids.Any())
                    {
                        _manager.Delete(uuids);
                    }
                    data.ReloadPage = true;
                });
            }
            return Json(resultEntry);
        }
        #endregion

        [HttpPost]
        public ActionResult GetRemoteFolders(string endPoint)
        {
            var resultEntry = new JsonResultData(ModelState);
            var ep = this._remoteEndPointSettingManager.Get(endPoint);
            var items = this._cmisSession.OpenSession(ep.CmisService, ep.CmisService, ep.CmisPassword).GetFolderTrees(ep.RemoteRepositoryId);
            var folders = new List<string>();
            foreach (var item in items)
            {
                folders.Add(item.Node.Value);
                folders.AddRange(RecursionFolders(item));
            }
            resultEntry.Model = folders.Select(it => new
            {
                id = it,
                text = it
            }).ToArray();
            return Json(resultEntry);
        }

        private IEnumerable<string> RecursionFolders(TreeNode<KeyValuePair<string, string>> treeNode)
        {
            var list = new List<string>();
            if (treeNode.Children != null)
            {
                foreach (var child in treeNode.Children)
                {
                    list.Add(child.Node.Value);
                    list.AddRange(RecursionFolders(child));
                }
            }
            return list;
        }
    }
}
