using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using Kooboo.CMS.Sites.View;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class ModuleHttpResponse : HttpResponseWrapper
    {
        private ModuleContext moduleContext;

        public ModuleHttpResponse(HttpResponse httpResponse, ModuleContext moduleContext)
            : base(httpResponse)
        {
            this.moduleContext = moduleContext;
        }
        //public override string ApplyAppPathModifier(string virtualPath)
        //{
        //    var routeValues = Page_Context.Current.PageRequestContext.ModuleUrlContext.GetRouteValuesWithModuleUrl(modulePosition.PagePositionId, virtualPath, modulePosition.Exclusive);

        //    return Page_Context.Current.FrontUrl.PageUrl(Page_Context.Current.PageRequestContext.Page.FullName, routeValues).ToString();

        //    //return virtualPath;
        //    //var removedApplicationPath = virtualPath;
        //    //var applicationPath = ModuleRequestContext.HttpContext.Request.ApplicationPath;
        //    //if (applicationPath != "/")
        //    //{
        //    //    removedApplicationPath = virtualPath.Replace(applicationPath, "");
        //    //}

        //    //return ModuleRequestContext.ModuleUrlConvertor.ToPageUrl(ModuleRequestContext.ParentPageContext.UrlHelper
        //    //    , ModuleRequestContext.PageRouteData, positionId, HttpUtility.UrlDecode(removedApplicationPath));
        //}
    }
}
