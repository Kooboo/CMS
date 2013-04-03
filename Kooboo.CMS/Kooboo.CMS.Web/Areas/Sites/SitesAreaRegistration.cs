using System.Web.Mvc;
using System.IO;

namespace Kooboo.CMS.Web.Areas.Sites
{
    public class SitesAreaRegistration : AreaRegistration
    {
        public static string SiteAreaName = "Sites";
        public override string AreaName
        {
            get
            {
                return SiteAreaName;
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            //context.MapRoute(
            //    "Sites_Content",
            //    "Contents/{controller}/{action}/{repositoryName}/{name}",
            //    new { action = "Index", repositoryName = UrlParameter.Optional, name = UrlParameter.Optional }
            //    , new { controller = "setting|schema|content|TextContent|BinaryContent" }
            //    , new[] { "Kooboo.CMS.Web.Areas.Contents.Controllers" }
            //);
            context.MapRoute("Admin", "Admin", new { controller = "Home", action = "Index", area = "Sites" });
            context.MapRoute(
                "Sites_default",
                "Sites/{controller}/{action}", //{siteName}/{name}  
                new { action = "Index" }//, siteName = UrlParameter.Optional, name = UrlParameter.Optional 
                , null
                , new[] { "Kooboo.CMS.Web.Areas.Sites.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );


            Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(AreaName, Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "Menu.config"));
            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "WebResources.config"));
        }
    }
}
