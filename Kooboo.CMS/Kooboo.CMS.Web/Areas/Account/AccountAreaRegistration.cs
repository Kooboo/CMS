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
using Kooboo.Web.Mvc;

namespace Kooboo.CMS.Web.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Account";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {            
            context.MapRoute(
                "Account_default",
                "Account/{controller}/{action}/{id}",
                new { controller = "LogOn", action = "Index", id = UrlParameter.Optional }
                , null
                , new[] { "Kooboo.CMS.Web.Areas.Account.Controllers", "Kooboo.Web.Mvc", "Kooboo.Web.Mvc.WebResourceLoader" }
            );

            Kooboo.Web.Mvc.Menu.MenuFactory.RegisterAreaMenu(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "Menu.config"));
            Kooboo.Web.Mvc.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "WebResources.config"));
        }
    }
}
