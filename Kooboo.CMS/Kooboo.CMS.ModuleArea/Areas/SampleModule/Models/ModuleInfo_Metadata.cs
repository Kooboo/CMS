﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using System.ComponentModel.DataAnnotations;
using Kooboo.Web.Mvc;
using System.Web.Mvc;
using System.Web.Routing;
using Kooboo.CMS.Sites.Models;

namespace Kooboo.CMS.ModuleArea.Models
{
    public class ModuleInfo_Metadata
    {
        public ModuleInfo_Metadata()
        {

        }
        public ModuleInfo_Metadata(string moduleName, string siteName)
        {
            var moduleInfo = ModuleInfo.Get(moduleName);

            this.ModuleName = moduleInfo.ModuleName;
            this.Version = moduleInfo.Version;
            this.KoobooCMSVersion = moduleInfo.KoobooCMSVersion;

            this.Settings = ModuleInfo.Get(moduleName).GetModuleSettings(new Site(siteName));
        }
        public string ModuleName { get; set; }
        public string Version { get; set; }
        public string KoobooCMSVersion { get; set; }
        public ModuleSettings Settings { get; set; }
    }

    public class ThemesDatasource : ISelectListDataSource
    {
        public IEnumerable<SelectListItem> GetSelectListItems(RequestContext requestContext, string filter)
        {
            var moduleName = Kooboo.Web.Mvc.AreaHelpers.GetAreaName(requestContext.RouteData);
            return Kooboo.CMS.Sites.Services.ServiceFactory.ModuleManager.AllThemes(moduleName).Select(it => new SelectListItem()
            {
                Text = it,
                Value = it
            });
        }
    }
    public class ModuleSettings_Metadata
    {
        [DataSource(typeof(ThemesDatasource))]
        [UIHint("DropDownList")]
        public string ThemeName { get; set; }

        public Entry Entry { get; set; }

        [UIHint("Dictionary")]
        public Dictionary<string, string> CustomSettings { get; set; }
    }
}