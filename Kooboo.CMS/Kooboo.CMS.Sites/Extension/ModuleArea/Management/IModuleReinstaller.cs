#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Extension.Management;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;

namespace Kooboo.CMS.Sites.Extension.ModuleArea.Management
{
    /// <summary>
    /// upgrade steps:
    /// 1. Upgrade the module area file(Delete the older folder and unzip the new module files,copy the new files to bin folder) 
    /// 2. Run upgrading event
    /// 3. Run the upgradation files(1.0.0.0-2.0.0.0.sql,etc)
    /// </summary>
    public interface IModuleReinstaller
    {
        // <summary>
        /// 上传Module的动作：
        /// 1. 将module文件解压到一个临时安装目录
        /// 2. 检查即将安装的module的Bin目录文件是否与系统存在的文件有冲突。并且返回冲突列表显示在页面上给用户确认。
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="moduleStream">The module stream.</param>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        UploadModuleResult Upload(string moduleName, Stream moduleStream, string user);

        /// <summary>
        /// Copies the assemblies.
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="overrideFiles">if set to <c>true</c> [override files].</param>
        void CopyAssemblies(string moduleName, bool @overrideFiles);

        /// <summary>
        /// 执行安装动作
        /// 1. 将临时安装目录的文件拷贝到Areas目录，并且Bin目录拷贝到系统Bin目录
        /// 2. 执行安装事件。
        /// </summary>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="controllerContext">The controller context.</param>
        /// <param name="user">The user.</param>
        void RunReinstallation(string moduleName, ControllerContext controllerContext, string user);

        IPath GetTempInstallationPath(string moduleName);
    }
}
