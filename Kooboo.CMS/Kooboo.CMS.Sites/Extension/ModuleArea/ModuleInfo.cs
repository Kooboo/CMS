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
using System.Text;
using System.Runtime.Serialization;
using Kooboo.Runtime.Serialization;
using Kooboo.CMS.Sites.Models;
using System.IO;
using Kooboo.Globalization;
using Kooboo.Web.Url;
namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public class EntryOption
    {
        public string Name { get; set; }
        public Entry Entry { get; set; }
    }
    [DataContract]
    public class ModuleInfo : Kooboo.CMS.Common.Persistence.Non_Relational.IIdentifiable
    {
        #region ModuleInfo properties
        public string ModuleName { get; set; }

        [DataMember(Order = 1)]
        public string Version { get; set; }

        [DataMember(Order = 3)]
        public string KoobooCMSVersion { get; set; }

        [DataMember(Order = 7)]
        public ModuleSettings DefaultSettings { get; set; }

        [DataMember(Order = 9)]
        public EntryOption[] EntryOptions { get; set; }

        [DataMember]
        public string InstallingTemplate { get; set; }

        [DataMember]
        public string UninstallingTemplate { get; set; }
        #endregion

        #region GetModuleInfo
        public static ModuleInfo Get(string moduleName)
        {
            ModulePath modulePath = new ModulePath(moduleName);
            if (!Directory.Exists(modulePath.PhysicalPath))
            {
                throw new Exception(string.Format("The module does not exist.Module name:{0}".Localize(), moduleName));
            }
            ModuleItemPath moduleInfoPath = GetModuleInfoPath(moduleName);
            var moduleInfo = DataContractSerializationHelper.Deserialize<ModuleInfo>(moduleInfoPath.PhysicalPath);
            moduleInfo.ModuleName = moduleName;
            return moduleInfo;
        }
        #endregion

        #region ModuleInfoFileName
        public static string ModuleInfoFileName = "module.config";
        #endregion


        #region GetModuleSettings
        public void SaveModuleSettings(Site site, ModuleSettings moduleSettings)
        {
            var settingFile = GetModuleSettingFile(site);
            DataContractSerializationHelper.Serialize(moduleSettings, settingFile);
        }
        public ModuleSettings GetModuleSettings(Site site)
        {
            var settingFile = GetModuleSettingFile(site);

            if (File.Exists(settingFile))
            {
                return DataContractSerializationHelper.Deserialize<ModuleSettings>(settingFile);
            }
            else
            {
                return this.DefaultSettings;
            }
        }

        private string GetModuleSettingFile(Site site)
        {
            var dataPath = GetModuleDataPath(site);
            var settingFile = Path.Combine(dataPath.PhysicalPath, "settings.config");
            return settingFile;
        }

        public IPath GetModuleDataPath(Site site)
        {
            var path = new CommonPath()
            {
                PhysicalPath = Path.Combine(site.PhysicalPath, "Modules", ModuleName),
                VirtualPath = UrlUtility.Combine(site.VirtualPath, "Modules", ModuleName)
            };
            return path;
        }
        #endregion

        #region GetModuleInfoPath
        public static ModuleItemPath GetModuleInfoPath(string moduleName)
        {
            return new ModuleItemPath(moduleName, ModuleInfoFileName);
        }

        #endregion

        #region Save ModuleInfo
        public static void Save(ModuleInfo moduleInfo)
        {
            ModuleItemPath moduleInfoPath = GetModuleInfoPath(moduleInfo.ModuleName);
            DataContractSerializationHelper.Serialize(moduleInfo, moduleInfoPath.PhysicalPath);
        }
        #endregion

        #region Obsolete methods
        
        #region SaveModuleSetting/GetSiteModuleSettings

        public static void SaveModuleSetting(string moduleName, string siteName, ModuleSettings moduleSettings)
        {
            var siteModuleSettingFile = GetSiteModuleSettingFile(moduleName, siteName);

            DataContractSerializationHelper.Serialize(moduleSettings, siteModuleSettingFile);
        }

        private static string GetSiteModuleSettingFile(string moduleName, string siteName)
        {
            Site site = new Site(siteName);
            var siteModulesPath = Path.Combine(site.PhysicalPath, "Modules");
            var siteModuleNamePath = Path.Combine(siteModulesPath, moduleName);
            var siteModuleSettingFile = Path.Combine(siteModuleNamePath, "settings.config");
            return siteModuleSettingFile;
        }
        [Obsolete("Use GetModuleSettings(Site site)")]
        public static ModuleSettings GetSiteModuleSettings(string moduleName, string siteName)
        {
            if (!string.IsNullOrEmpty(siteName))
            {
                var siteModuleSettingFile = GetSiteModuleSettingFile(moduleName, siteName);

                if (File.Exists(siteModuleSettingFile))
                {
                    return DataContractSerializationHelper.Deserialize<ModuleSettings>(siteModuleSettingFile);
                }
            }
            return Get(moduleName).DefaultSettings;
        }
        #endregion

        
        #endregion

        #region IIdentifiable Members
        public string UUID
        {
            get
            {
                return this.ModuleName;
            }
            set
            {
                this.ModuleName = value;
            }
        }
        #endregion
    }
}
