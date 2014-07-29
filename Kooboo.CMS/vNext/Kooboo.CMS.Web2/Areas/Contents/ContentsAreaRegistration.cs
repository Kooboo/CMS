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

using Kooboo.Common.ObjectContainer;
using Kooboo.CMS.Account.Services;
using Kooboo.CMS.Account.Models;
using Kooboo.Common.Web;

namespace Kooboo.CMS.Web2.Areas.Contents
{

    public class ContentsAreaRegistration : AreaRegistrationEx
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
                , new[] { "Kooboo.CMS.Web2.Areas.Contents.Controllers", "Kooboo.Web.Mvc", "Kooboo.Common.Web.WebResourceLoader" }
            );

            Kooboo.Common.Web.Menu.MenuFactory.RegisterAreaMenu(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "Menu.config"));
            Kooboo.Common.Web.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "WebResources.config"));

            #region RegisterPermissions
            var roleManager = Kooboo.Common.ObjectContainer.EngineContext.Current.Resolve<RoleManager>();
            roleManager.AddPermission(Permission.Contents_SettingPermission);
            roleManager.AddPermission(Permission.Contents_SchemaPermission);
            roleManager.AddPermission(Permission.Contents_FolderPermission);
            roleManager.AddPermission(Permission.Contents_ContentPermission);
            roleManager.AddPermission(Permission.Contents_BroadcastingPermission);
            roleManager.AddPermission(Permission.Contents_WorkflowPermission);
            roleManager.AddPermission(Permission.Contents_SearchSettingPermission);
            roleManager.AddPermission(Permission.Contents_HtmlBlockPermission);

            #endregion

            base.RegisterArea(context);
        }
    }
}
