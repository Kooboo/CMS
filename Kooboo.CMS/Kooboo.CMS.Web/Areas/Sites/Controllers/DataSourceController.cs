using Kooboo.Web;
using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Services;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "DataSource", Order = 1)]
    public class DataSourceController : ManageControllerBase<DataSourceSetting, DataSourceSettingManager>
    {
        #region .ctor
        DataSourceSettingManager _manager;
        IDataSourceDesigner[] _designers;
        public DataSourceController(DataSourceSettingManager manager, IDataSourceDesigner[] designers)
            : base(manager)
        {
            this._manager = manager;
            this._designers = designers;
        }
        #endregion

        #region OnActionExecutiong
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Designers = this._designers;
            base.OnActionExecuting(filterContext);
        }
        #endregion

        #region Create
        public override System.Web.Mvc.ActionResult Create(DataSourceSetting model)
        {
            var designer = HttpContext.Request.QueryString["Designer"];

            var designerObject = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<IDataSourceDesigner>(designer);

            ViewBag.Designer = designerObject;

            return base.Create(model);
        }

        [HttpPost]
        public override ActionResult Create(DataSourceSetting model, string @return)
        {
            //这里未来应该是可以提取一个接口，可追踪修改的接口
            model.UtcCreationDate = DateTime.UtcNow;
            model.UtcLastestModificationDate = DateTime.UtcNow;
            model.LastestEditor = User.Identity.Name;
            return base.Create(model, @return);
        }
        #endregion

        #region Edit
        public override ActionResult Edit(string uuid)
        {
            var o = Get(uuid);

            var designer = _designers.Where(it => it.IsEditorFor(o.DataSource)).FirstOrDefault();

            ViewBag.Designer = designer;

            return View(o);
        }
        [HttpPost]
        public override ActionResult Edit(DataSourceSetting newModel, string uuid, string @return)
        {
            newModel.UtcCreationDate = Get(uuid).UtcCreationDate;
            newModel.UtcLastestModificationDate = DateTime.UtcNow;
            newModel.LastestEditor = User.Identity.Name;
            return base.Edit(newModel, uuid, @return);
        }
        #endregion

        #region IsDataNameAvailable
        public virtual ActionResult IsDataNameAvailable(string dataName, string old_Key)
        {
            if (old_Key == null || !dataName.EqualsOrNullEmpty(old_Key, StringComparison.CurrentCultureIgnoreCase))
            {
                if (Manager.Get(Site, dataName) != null)
                {
                    return Json("The name already exists.", JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region import/export
        public void Export(DataSourceSetting[] model)
        {
            var fileName = GetZipFileName();
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

        protected string GetZipFileName()
        {
            return "DataSources.zip";
        }

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

        #region Relations
        public virtual ActionResult Relations(string uuid)
        {
            var model = Manager.Relations(new DataSourceSetting(Site, uuid));
            return View("Relations", model);
        }
        #endregion

        #region Embedded
        public ActionResult Embedded(string uuid)
        {
            ViewBag.AllDataSourceSettings = Manager.All(Site, null);
            var dataSourceSetting = Manager.Get(Site, uuid);
            return View(dataSourceSetting);
        }
        #endregion

    }
}