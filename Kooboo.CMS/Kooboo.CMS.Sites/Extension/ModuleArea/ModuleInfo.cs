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
    [DataContract]
    public class EntryOption
    {
        [DataMember(EmitDefaultValue = false)]
        public string Name { get; set; }
        [DataMember(EmitDefaultValue = false)]
        public Entry Entry { get; set; }
    }
    [DataContract]
    public class ModuleInfo : Kooboo.CMS.Common.Persistence.Non_Relational.IIdentifiable
    {
        public static string ModuleInfoFileName = "module.config";
        #region ModuleInfo properties
        [DataMember(Order = 0)]
        public string ModuleName { get; set; }

        [DataMember(Order = 1)]
        public string Version { get; set; }

        [DataMember(Order = 3)]
        public string KoobooCMSVersion { get; set; }

        [DataMember(Order = 5)]
        public string InstallingTemplate { get; set; }

        [DataMember(Order = 6)]
        public string UninstallingTemplate { get; set; }

        [DataMember(Order = 7)]
        public string ReinstallingTemplate { get; set; }

        [DataMember(Order = 8)]
        public ModuleSettings DefaultSettings { get; set; }

        [DataMember(Order = 9)]
        public EntryOption[] EntryOptions { get; set; }


        #endregion

        #region Get/Save
        public static ModuleInfo Get(string moduleName)
        {
            ModulePath modulePath = new ModulePath(moduleName);
            if (!Directory.Exists(modulePath.PhysicalPath))
            {
                return null;
            }
            string moduleInfoPath = GetModuleInfoPath(moduleName);
            var moduleInfo = DataContractSerializationHelper.Deserialize<ModuleInfo>(moduleInfoPath);
            moduleInfo.ModuleName = moduleName;
            return moduleInfo;
        }
        public static ModuleInfo Get(Stream stream)
        {
            return (ModuleInfo)DataContractSerializationHelper.Deserialize(typeof(ModuleInfo), null, stream);
        }
        private static string GetModuleInfoPath(string moduleName)
        {
            return new ModuleContext(moduleName).ModulePath.GetModuleInstallationFilePath(ModuleInfoFileName).PhysicalPath;
        }
        public static void Save(ModuleInfo moduleInfo)
        {
            var moduleInfoPath = GetModuleInfoPath(moduleInfo.ModuleName);
            DataContractSerializationHelper.Serialize(moduleInfo, moduleInfoPath);
        }
        #endregion

        #region GetModuleSettings
        [Obsolete]
        public void SaveModuleSettings(Site site, ModuleSettings moduleSettings)
        {
            throw new NotSupportedException("Use ModuleContext.SetModuleSettings()");
            //var settingFile = GetModuleSettingFile(site);
            //DataContractSerializationHelper.Serialize(moduleSettings, settingFile);
        }
        [Obsolete]
        public ModuleSettings GetModuleSettings(Site site)
        {
            throw new NotSupportedException("Use ModuleContext.GetModuleSetting()");
            //var settingFile = GetModuleSettingFile(site);

            //if (File.Exists(settingFile))
            //{
            //    return DataContractSerializationHelper.Deserialize<ModuleSettings>(settingFile);
            //}
            //else
            //{
            //    return this.DefaultSettings;
            //}
        }

        //private string GetModuleSettingFile(Site site)
        //{
        //    return new ModulePathHelper(ModuleName, site).GetModuleLocalFilePath("settings.config").PhysicalPath;
        //}
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
