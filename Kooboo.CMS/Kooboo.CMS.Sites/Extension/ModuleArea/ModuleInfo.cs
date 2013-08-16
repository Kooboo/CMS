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
using Kooboo.CMS.Common;
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
            return new ModulePathHelper(ModuleName, site).GetModuleLocalFilePath("settings.config").PhysicalPath;
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
