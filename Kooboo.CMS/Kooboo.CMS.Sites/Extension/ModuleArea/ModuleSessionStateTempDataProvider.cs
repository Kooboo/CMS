using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using System.Web;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModuleSessionStateTempDataProvider : ITempDataProvider
    {
        // Fields
        internal const string TempDataSessionStateKey = "__ModuleControllerTempData";

        // Methods
        public virtual IDictionary<string, object> LoadTempData(ControllerContext controllerContext)
        {
            HttpContextBase httpContext = controllerContext.HttpContext;
            if (httpContext.Session == null)
            {
                throw new InvalidOperationException("SessionStateTempDataProvider_SessionStateDisabled");
            }
            Dictionary<string, object> dictionary = httpContext.Session[TempDataSessionStateKey] as Dictionary<string, object>;
            if (dictionary != null)
            {
                httpContext.Session.Remove(TempDataSessionStateKey);
                return dictionary;
            }
            return new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);
        }

        public virtual void SaveTempData(ControllerContext controllerContext, IDictionary<string, object> values)
        {
            HttpContextBase httpContext = controllerContext.HttpContext;
            if (httpContext.Session == null)
            {
                throw new InvalidOperationException("SessionStateTempDataProvider_SessionStateDisabled");
            }
            httpContext.Session[TempDataSessionStateKey] = values;
        }
    }
}
