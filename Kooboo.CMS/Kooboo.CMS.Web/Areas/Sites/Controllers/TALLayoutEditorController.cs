using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.Services;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Common.Persistence.Non_Relational;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "Layout", Order = 1)]
    public class TALLayoutEditorController : Kooboo.CMS.Sites.AreaControllerBase
    {
        #region ctor
        LayoutManager _layoutManager;
        Layout tempLayout = null;
        public TALLayoutEditorController(LayoutManager layoutManager)
        {
            _layoutManager = layoutManager;
        }
        #endregion
        public virtual ActionResult Preview(string layout)
        {
            if (string.IsNullOrEmpty(layout))
            {
                layout = Kooboo.UniqueIdGenerator.GetInstance().GetBase32UniqueId(5);
                tempLayout = new Layout(Site, layout)
                {
                    EngineName = "TAL",
                    Body = Kooboo.CMS.Sites.Services.ServiceFactory.LayoutItemTemplateManager.GetDefaultLayoutSample("TAL").Template
                };
                _layoutManager.Add(Site, tempLayout);
            }

            var page = new Page(Site, "TALEditor_Preview") { IsDummy = false, Layout = layout, EnableTheming = true, EnableScript = true };

            this.HttpContext.Items["TALDesign"] = "true";
            this.HttpContext.Items["TalLayoutEditor"] = "true";
            // new context
            var requestContext = new PageRequestContext(this.ControllerContext, Site, page, CMS.Sites.Web.FrontRequestChannel.Debug, "/");

            // init context
            Page_Context.Current.InitContext(requestContext, ControllerContext);

            // ret
            return ViewPage();
        }
        private ActionResult ViewPage()
        {
            var layout = (new Layout(Site, Page_Context.Current.PageRequestContext.Page.Layout).LastVersion()).AsActual();

            ViewResult viewResult = new FrontViewResult(ControllerContext, layout.FileExtension.ToLower(), layout.TemplateFileVirutalPath);

            if (viewResult != null)
            {
                viewResult.ViewData = this.ViewData;
                viewResult.TempData = this.TempData;
            }

            return viewResult;
        }

        protected override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);
            ClearTemp();
        }

        private void ClearTemp()
        {            
            if (tempLayout != null)
            {
                _layoutManager.Remove(Site, tempLayout);
            }
        }
        protected override void OnException(ExceptionContext filterContext)
        {
            ClearTemp();
            base.OnException(filterContext);
        }
    }
}