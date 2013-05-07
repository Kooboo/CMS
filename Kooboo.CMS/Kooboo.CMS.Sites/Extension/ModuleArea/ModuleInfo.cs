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
        public string ModuleName { get; set; }

        [DataMember(Order = 1)]
        public string Version { get; set; }

        [DataMember(Order = 3)]
        public string KoobooCMSVersion { get; set; }

        [DataMember(Order = 7)]
        public ModuleSettings DefaultSettings { get; set; }

        [DataMember(Order = 9)]
        public EntryOption[] EntryOptions { get; set; }

        public static ModuleInfo Get(string moduleName)
        {
            ModulePath modulePath = new ModulePath(moduleName);
            if (!Directory.Exists(modulePath.PhysicalPath))
            {
                throw new Exception(string.Format("The module does not exist.Module name:{0}".Localize(), moduleName));
            }
            ModuleEntryPath moduleInfoPath = GetModuleInfoPath(moduleName);
            var moduleInfo = DataContractSerializationHelper.Deserialize<ModuleInfo>(moduleInfoPath.PhysicalPath);
            moduleInfo.ModuleName = moduleName;
            return moduleInfo;
        }

        public static string ModuleInfoFileName = "module.config";

        public static ModuleEntryPath GetModuleInfoPath(string moduleName)
        {
            return new ModuleEntryPath(moduleName, ModuleInfoFileName);
        }

        public static void Save(ModuleInfo moduleInfo)
        {
            ModuleEntryPath moduleInfoPath = GetModuleInfoPath(moduleInfo.ModuleName);
            DataContractSerializationHelper.Serialize(moduleInfo, moduleInfoPath.PhysicalPath);
        }

        public static void SaveModuleSetting(string moduleName, string siteName, ModuleSettings moduleSettings)
        {
            var siteModuleSettingFile = GetSiteModuleSettingFile(moduleName, siteName);

            DataContractSerializationHelper.Serialize(moduleSettings, siteModuleSettingFile);
        }
        private static string GetSiteModuleSettingFile(string moduleName, string siteName)
        {
            Site site = new Site(siteName);
            var siteModulesPath = Path.Combine(site.PhysicalPath, "Modules");          
            var siteModuleSettingFile = Path.Combine(siteModulesPath, "settings.config");
            return siteModuleSettingFile;
        }
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
    }
}
