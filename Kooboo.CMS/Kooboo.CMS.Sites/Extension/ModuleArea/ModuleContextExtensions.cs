#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.Runtime.Serialization;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Sites.Extension.ModuleArea
{
    public static class ModuleContextExtensions
    {
        #region Module settings
        public static void SetModuleSettings(this ModuleContext moduleContext, ModuleSettings moduleSettings)
        {
            var settingFile = moduleContext.ModulePath.GetModuleLocalFilePath("settings.config").PhysicalPath;
            DataContractSerializationHelper.Serialize(moduleSettings, settingFile);
        }
        public static ModuleSettings GetModuleSettings(this ModuleContext moduleContext)
        {
            var settingFile = moduleContext.ModulePath.GetModuleLocalFilePath("settings.config").PhysicalPath;

            if (File.Exists(settingFile))
            {
                return DataContractSerializationHelper.Deserialize<ModuleSettings>(settingFile);
            }
            else
            {
                return moduleContext.ModuleInfo.DefaultSettings;
            }
        }
        #endregion
    }
}
