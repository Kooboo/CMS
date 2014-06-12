using Kooboo.CMS.Sites.Models;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Common.Persistence.Non_Relational;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Sites.Controllers
{
    [Kooboo.CMS.Web.Authorizations.Authorization(AreaName = "Sites", Group = "Development", Name = "View", Order = 1)]
    public class TALViewEditorController : Kooboo.CMS.Sites.AreaControllerBase
    {
        public virtual ActionResult Preview(string layout, string view, string position)
        {
            var page = new Page(Site, "TALEditor_Preview") { IsDummy = false, Layout = layout, EnableTheming = true, EnableScript = true };

            if (!string.IsNullOrEmpty(view) && !string.IsNullOrEmpty(position))
            {
                page.PagePositions.Add(new ViewPosition() { LayoutPositionId = position, ViewName = view, PagePositionId = Guid.NewGuid().ToString() });
            }

            this.HttpContext.Items["TALDesign"] = "true";
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
    }
}