using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    /// <summary>
    /// The base controller of front end
    /// The other option is to use the ModulActionFilter, it will allow you to inherit from the Controller instead of ModuleControllerBase
    /// </summary>
    public class ModuleControllerBase : Controller
    {
        static ModuleControllerBase()
        {

        }
        public ModuleControllerBase()
        {

        }

        public ModuleContext ModuleContext
        {
            get
            {
                return ((ModuleRequestContext)(ControllerContext.RequestContext)).ModuleContext;
            }
        }
        private bool enableTheming = true;
        public virtual bool EnableTheming
        {
            get
            {
                return enableTheming;
            }
            set
            {
                this.enableTheming = value;
            }
        }
        private bool enableScript = true;
        public virtual bool EnableScript
        {
            get
            {
                return enableScript;
            }
            set
            {
                enableScript = value;
            }
        }

        public ControllerContext PageControllerContext
        {
            get
            {
                return ((ModuleRequestContext)(ControllerContext.RequestContext)).PageControllerContext;
            }
        }

        protected override JsonResult Json(object data, string contentType, System.Text.Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            if (string.IsNullOrEmpty(contentType))
            {
                if (Request.ContentType.Contains("multipart/form-data;"))
                {
                    contentType = "text/plain";
                }
            }
            return base.Json(data, contentType, contentEncoding, behavior);
        }
    }
}
