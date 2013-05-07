using Kooboo.CMS.Sites.Web;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Controllers.ActionFilters
{
    public class CustomOutputTextWriterFilter : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            filterContext.HttpContext.Response.Output = new OutputTextWriterWrapper(filterContext.HttpContext.Response.Output);
        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            base.OnResultExecuted(filterContext);

            var outputTextWriterWrapper = filterContext.HttpContext.Response.Output as OutputTextWriterWrapper;
            if (outputTextWriterWrapper != null)
            {
                outputTextWriterWrapper.Render(filterContext.HttpContext.Response);
            }
        }
    }
}
