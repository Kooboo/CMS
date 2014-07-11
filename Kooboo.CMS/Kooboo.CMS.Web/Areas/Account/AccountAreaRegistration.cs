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

using Kooboo.Common.ObjectContainer;
using Kooboo.Common.Web;
using Kooboo.Common;

namespace Kooboo.CMS.Web.Areas.Account
{
    public class AccountAreaRegistration : AreaRegistrationEx
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
                , new[] { "Kooboo.CMS.Web.Areas.Account.Controllers", "Kooboo.Web.Mvc", "Kooboo.Common.Web.WebResourceLoader" }
            );

            Kooboo.Common.Web.Menu.MenuFactory.RegisterAreaMenu(AreaName, AreaHelpers.CombineAreaFilePhysicalPath(AreaName, "Menu.config"));
            Kooboo.Common.Web.WebResourceLoader.ConfigurationManager.RegisterSection(AreaName, Path.Combine(Settings.BaseDirectory, "Areas", AreaName, "WebResources.config"));


            base.RegisterArea(context);
        }
    }
}
