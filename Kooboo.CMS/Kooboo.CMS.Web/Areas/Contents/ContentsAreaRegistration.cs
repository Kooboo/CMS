#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System.Web.Mvc;
using System.IO;
using System.Web.Routing;
using Kooboo.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Contents
{

    public class ContentsAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Contents";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //var metaweblogRoute = new Route("Contents/metaweblog", null, null, new MetaWeblogRouteHandler());
            //context.Routes.Add(metaweblogRoute);

            context.MapRoute(
                "Contents_default",
                "Contents/{controller}/{action}",///{repositoryName}/{name}
                new { action = "Index" }
                , null
                , new[] { "Kooboo.CMS.Web.Areas.Contents.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );

            Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "Menu.config"));
            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "WebResources.config"));
        }
    }
}
