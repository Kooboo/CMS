#region License
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
using System.Web.Mvc;
using Kooboo.CMS.Sites.Extension.ModuleArea;
using Kooboo.CMS.ModuleArea.Models;
using Kooboo.Globalization;
using Kooboo.CMS.Sites;
using Kooboo.CMS.Common;
namespace Kooboo.CMS.ModuleArea.Controllers
{
    public class AdminController : AdminControllerBase
    {
        public ActionResult Index(string siteName)
        {
            ModuleInfo_Metadata moduleInfo = new ModuleInfo_Metadata(ModuleName, siteName);

            return View(moduleInfo);
        }

        [HttpPost]
        public ActionResult Index(string siteName, ModuleInfo_Metadata moduleInfo)
        {
            JsonResultData resultEntry = new JsonResultData(ModelState);
            try
            {
                if (ModelState.IsValid)
                {
                    ModuleInfo.SaveModuleSetting(ModuleName, siteName, moduleInfo.Settings);
                    resultEntry.AddMessage("Module setting has been changed.".Localize());
                }

            }
            catch (Exception e)
            {
                resultEntry.AddException(e);
            }
            return Json(resultEntry);
        }

        public ActionResult InitializeModule(string siteName)
        {
            return View();
        }

        public ActionResult GenerateModuleInfo()
        {
            ModuleInfo moduleInfo = new ModuleInfo();
            moduleInfo.ModuleName = "SampleModule";
            moduleInfo.Version = "4.0.0.0";
            moduleInfo.KoobooCMSVersion = "4.0.0.0";
            moduleInfo.DefaultSettings = new ModuleSettings()
            {
                ThemeName = "Default",
                Entry = new Entry()
                {
                    Controller = "Home",
                    Action = "Index"
                }
            };
            moduleInfo.EntryOptions = new EntryOption[]{
                new EntryOption(){ Name="NewsList",Entry = new Entry{ Controller="News",Action ="Index"}},
                new EntryOption(){ Name="NewsCategories",Entry = new Entry{ Controller="News",Action ="Categories"}},
                new EntryOption(){Name="ArticleCategories",Entry=new Entry{Controller="Article",Action="Categories"}},
                new EntryOption(){Name="ArticleList",Entry=new Entry{Controller="Article",Action="List"}},
            };
            moduleInfo.DefaultSettings.CustomSettings = new Dictionary<string, string>();
            moduleInfo.DefaultSettings.CustomSettings["Setting1"] = "Value1";
            ModuleInfo.Save(moduleInfo);

            return View();
        }
    }
}
