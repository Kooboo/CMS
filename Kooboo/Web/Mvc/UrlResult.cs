using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.Web.Mvc
{
    public class UrlResult : ActionResult
    {
        public UrlResult(string physicalPath)
        {
            this.PhysicalPath = physicalPath;
        }

        public string PhysicalPath
        {
            get;
            set;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            context.HttpContext.Response.ContentType = "text/plain";
            context.HttpContext.Response.Write(this.ResolveClientUrl(context));
        }

        string ResolveClientUrl(ControllerContext context)
        {
            var root = context.HttpContext.Server.MapPath("~");

            var relativePath = this.PhysicalPath.Substring(root.Length - 1, this.PhysicalPath.Length - root.Length + 1);

            return relativePath.Replace("\\", "/");

        }
    }
}
