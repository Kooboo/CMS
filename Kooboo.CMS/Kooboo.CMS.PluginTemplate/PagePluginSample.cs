using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Kooboo.CMS.Sites.Extension;
using Kooboo.CMS.Sites.View;

namespace Kooboo.CMS.PluginTemplate
{
    public class PagePluginSample : IPagePlugin
    {
        #region IPagePlugin Members

        public System.Web.Mvc.ActionResult Execute(Page_Context pageViewContext, PagePositionContext positionContext)
        {
            //pageViewContext.ControllerContext.HttpContext.Response.Write("Sample plugin executed.<br/>");
            return null;
        }

        #endregion

        public string Description
        {
            get { return "Sample plugin"; }
        }
    }
}
