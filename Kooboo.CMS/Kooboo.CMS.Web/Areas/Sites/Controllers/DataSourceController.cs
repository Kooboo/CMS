using Kooboo.CMS.Sites.DataSource;
using Kooboo.CMS.Sites.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

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
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            ViewBag.Designers = this._designers;
            base.OnActionExecuting(filterContext);
        }

        public override System.Web.Mvc.ActionResult Create(DataSourceSetting model)
        {
            var designer = HttpContext.Request.QueryString["Designer"];

            var designerObject = Kooboo.CMS.Common.Runtime.EngineContext.Current.Resolve<IDataSourceDesigner>(designer);

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
    }
}